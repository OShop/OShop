using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingInfoResolver : IShoppingCartBuilder {

        public Int32 Priority {
            get { return 50; }
        }

        public void BuildCart(IShoppingCartService ShoppingCartService, ref ShoppingCart Cart) {
            foreach (var item in Cart.Items) {
                if (item.Item.Content != null) {
                    var shippingPart = item.Item.Content.As<ShippingPart>();
                    if (shippingPart != null) {
                        item.ShippingInfo = shippingPart;
                    }
                }
            }
        }
    }
}