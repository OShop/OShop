using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Extensions;
using OShop.Models;
using OShop.Services;
using System;
using System.Linq;
using System.Web.Routing;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Orders")]
    public class OrderPartHandler : ContentHandler {
        private readonly IRepository<OrderDetailRecord> _orderDetailsRepository;

        public OrderPartHandler(
            IRepository<OrderPartRecord> repository,
            IContentManager contentManager,
            IRepository<OrderDetailRecord> orderDetailsRepository,
            IOrdersService ordersService,
            IRepository<OrderAddressRecord> orderAddressRepository) {
            _orderDetailsRepository = orderDetailsRepository;

            Filters.Add(StorageFilter.For(repository));

            OnActivated<OrderPart>((context, part) => {
                // Details
                part._details.Loader(details => _orderDetailsRepository.Fetch(d => d.OrderId == part.Id)
                    .Select(d => new OrderDetail(d))
                    .ToList());

                // Order total
                part._orderTotal.Loader(orderTotal => BuildOrderTotal(part));

                // BillingAddress
                part._billingAddress.Loader(shippingAddress => orderAddressRepository.Get(part.BillingAddressId));
            });

            OnLoading<OrderPart>((context, part) => {
                // Order total
                part._orderTotal.Loader(orderTotal => part.Retrieve(x => x.OrderTotal));
            });

            OnCreating<OrderPart>((context, part) => {
                if (String.IsNullOrWhiteSpace(part.Reference)) {
                    part.Reference = ordersService.BuildOrderReference();
                }
            });

            OnCreated<OrderPart>((context, part) => {
                // Order total
                part.OrderTotal = BuildOrderTotal(part);

                SaveDetails(part);
                part.BillingAddressId = orderAddressRepository.CreateOrUpdate(part.BillingAddress);
            });

            OnUpdated<OrderPart>((context, part) => {
                // Order total
                part.OrderTotal = BuildOrderTotal(part);

                SaveDetails(part);
                part.BillingAddressId = orderAddressRepository.CreateOrUpdate(part.BillingAddress);
            });
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            var order = context.ContentItem.As<OrderPart>();

            if (order == null)
                return;

            // Admin link shows customer details
            context.Metadata.AdminRouteValues = new RouteValueDictionary {
                {"Area", "OShop"},
                {"Controller", "OrdersAdmin"},
                {"Action", "Detail"},
                {"Id", order.Id}
            };
        }

        private void SaveDetails(OrderPart part) {
            if (part.Id <= 0) {
                return;
            }

            var oldDetails = _orderDetailsRepository.Fetch(d => d.OrderId == part.Id);
            foreach (var detail in part.Details.Where(d => !oldDetails.Where(od => od.Id == d.Id).Any())) {
                // New details
                var newRecord = detail.Record;
                newRecord.OrderId = part.Id;
                _orderDetailsRepository.Create(newRecord);
            }
            foreach (var detail in part.Details.Join(oldDetails, d => d.Id, od => od.Id, (d, od) => new { updated = d, stored = od})) {
                // Updated details
                if (detail.updated.Quantity <= 0) {
                    _orderDetailsRepository.Delete(detail.stored);
                }
                else {
                    _orderDetailsRepository.Update(detail.updated.Record);
                }
            }
            foreach (var removed in oldDetails.Where(od => !part.Details.Where(d => d.Id == od.Id).Any())) {
                // Removed details
                _orderDetailsRepository.Delete(removed);
            }
        }

        private static decimal BuildOrderTotal(OrderPart part) {
            return part.ContentItem.Parts.Select(p => p as IOrderSubTotal != null ? (p as IOrderSubTotal).SubTotal : 0).Sum();
        }
    }
}