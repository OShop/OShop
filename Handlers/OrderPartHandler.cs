using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;
using OShop.Services;
using System;
using System.Web.Routing;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Orders")]
    public class OrderPartHandler : ContentHandler {

        public OrderPartHandler(
            IRepository<OrderPartRecord> repository,
            IOrdersService ordersService
            ) {

            Filters.Add(StorageFilter.For(repository));

            OnCreating<OrderPart>((context, part) => {
                if (String.IsNullOrWhiteSpace(part.Reference)) {
                    part.Reference = ordersService.BuildOrderReference();
                }
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
    }
}