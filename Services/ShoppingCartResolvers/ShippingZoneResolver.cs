using Orchard.Environment.Extensions;
using OShop.Helpers;
using OShop.Models;
using System;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingZoneResolver : IShoppingCartResolver {
        public Int32 Priority {
            get { return 990; }
        }

        public void ResolveCart(ref ShoppingCart Cart) {
            if (Cart.State != null && Cart.State.ShippingZoneRecord != null && Cart.State.ShippingZoneRecord.Enabled) {
                Cart.ShippingZone = Cart.State.ShippingZoneRecord;
            }
            else if (Cart.Country != null && Cart.Country.ShippingZoneRecord != null && Cart.Country.ShippingZoneRecord.Enabled) {
                Cart.ShippingZone = Cart.Country.ShippingZoneRecord;
            }
            else {
                Cart.ShippingZone = null;
            }
        }
    }
}