using Orchard.Environment.Extensions;
using Orchard.ContentManagement;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingInfoProvider : IShippingInfoProvider {
        public List<ItemShippingInfo> GetShippingInfos(IEnumerable<ShoppingCartItem> CartItems) {
            List<ItemShippingInfo> result = new List<ItemShippingInfo>();

            foreach (var item in CartItems) {
                if(item.Item.Content != null) {
                    var shippingPart = item.Item.Content.As<ShippingPart>();
                    if (shippingPart != null) {
                        result.Add(new ItemShippingInfo() {
                            Quantity = item.Quantity,
                            ShippingInfo = shippingPart
                        });
                    }
                }
            }

            return result;
        }
    }
}