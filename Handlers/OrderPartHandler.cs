using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Events;
using OShop.Extensions;
using OShop.Models;
using OShop.Services;
using System;
using System.Linq;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Orders")]
    public class OrderPartHandler : ContentHandler {
        public OrderPartHandler(
            IRepository<OrderPartRecord> repository,
            IContentManager contentManager,
            IRepository<OrderDetailRecord> orderDetailsRepository,
            IOrdersService ordersService,
            IRepository<OrderAddressRecord> orderAddressRepository,
            IOrderEventHandler orderEventHandler) {

            Filters.Add(StorageFilter.For(repository));

            OnActivated<OrderPart>((context, part) => {
                // Details
                part._details.Loader(() => orderDetailsRepository.Fetch(d => d.OrderId == part.Id)
                    .Select(d => new OrderDetail(d))
                    .ToList());

                // Order total
                part._orderTotal.Loader(() => BuildOrderTotal(part));

                // BillingAddress
                part._billingAddress.Loader(() => orderAddressRepository.Get(part.BillingAddressId));
            });

            OnLoading<OrderPart>((context, part) => {
                // Order total
                part._orderTotal.Loader(() => part.Retrieve(x => x.OrderTotal));
            });

            OnCreating<OrderPart>((context, part) => {
                if (String.IsNullOrWhiteSpace(part.Reference)) {
                    part.Reference = ordersService.BuildOrderReference();
                }
            });

            OnCreated<OrderPart>((context, part) => {
                // Order total
                part.OrderTotal = BuildOrderTotal(part);

                SaveDetails(part, orderDetailsRepository, orderEventHandler);
                part.BillingAddressId = orderAddressRepository.CreateOrUpdate(part.BillingAddress);

                orderEventHandler.OrderCreated(context.ContentItem);
            });

            OnUpdating<OrderPart>((context, part) => {
                // Status
                part.OriginalStatus = part.OrderStatus;
            });

            OnUpdated<OrderPart>((context, part) => {
                // Order total
                part.OrderTotal = BuildOrderTotal(part);

                SaveDetails(part, orderDetailsRepository, orderEventHandler);
                part.BillingAddressId = orderAddressRepository.CreateOrUpdate(part.BillingAddress);

                if(part.OrderStatus != part.OriginalStatus) {
                    switch (part.OrderStatus) {
                        case OrderStatus.Canceled:
                            orderEventHandler.OrderCanceled(context.ContentItem);
                            break;
                        case OrderStatus.Completed:
                            orderEventHandler.OrderCompleted(context.ContentItem);
                            break;
                    }
                }
            });
        }

        private void SaveDetails(OrderPart part, IRepository<OrderDetailRecord> detailsRepository, IOrderEventHandler orderEventHandler) {
            if (part.Id <= 0) {
                return;
            }

            var oldDetails = detailsRepository.Fetch(d => d.OrderId == part.Id);
            foreach (var detail in part.Details.Where(d => !oldDetails.Where(od => od.Id == d.Id).Any())) {
                // New details
                var newRecord = detail.Record;
                newRecord.OrderId = part.Id;
                detailsRepository.Create(newRecord);
                orderEventHandler.OrderDetailCreated(part.ContentItem, newRecord);
            }
            foreach (var detail in part.Details.Join(oldDetails, d => d.Id, od => od.Id, (d, od) => new { updated = d, stored = od})) {
                // Updated details
                if (detail.updated.Quantity <= 0) {
                    detailsRepository.Delete(detail.stored);
                    orderEventHandler.OrderDetailDeleted(part.ContentItem, detail.stored);
                }
                else {
                    detailsRepository.Update(detail.updated.Record);
                    orderEventHandler.OrderDetailUpdated(part.ContentItem, detail.stored, detail.updated.Record);
                }
            }
            foreach (var removed in oldDetails.Where(od => !part.Details.Where(d => d.Id == od.Id).Any())) {
                // Removed details
                detailsRepository.Delete(removed);
                orderEventHandler.OrderDetailDeleted(part.ContentItem, removed);
            }
        }

        private static decimal BuildOrderTotal(OrderPart part) {
            return part.ContentItem.Parts.Select(p => p as IOrderSubTotal != null ? (p as IOrderSubTotal).SubTotal : 0).Sum();
        }
    }
}