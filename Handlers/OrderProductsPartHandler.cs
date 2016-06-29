using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using OShop.Models;
using System.Collections.Generic;
using System.Linq;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Products")]
    public class OrderProductsPartHandler : ContentHandler {
        public OrderProductsPartHandler() {
            OnActivated<OrderProductsPart>((context, part) => {
                part._productDetails.Loader(() => {
                    var orderPart = context.ContentItem.As<OrderPart>();
                    if (orderPart != null) {
                        return orderPart.Details.Where(d => d.DetailType == ProductPart.PartItemType);
                    }
                    else {
                        return new List<OrderDetail>();
                    }
                });
            });
        }
    }
}