using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using OShop.Helpers;
using OShop.Models;
using System;
using System.Web.Mvc;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingZoneResolver : IShoppingCartBuilder {
        public ShippingZoneResolver() {
        }

        public Int32 Priority {
            get { return 800; }
        }

        public void BuildCart(IShoppingCartService ShoppingCartService, ShoppingCart Cart) {
            var country = Cart.Properties["ShippingCountry"] as LocationsCountryRecord;
            var state = Cart.Properties["ShippingState"] as LocationsStateRecord;

            if (state != null && state.Enabled && state.ShippingZoneRecord != null) {
                Cart.Properties["ShippingZone"] = state.ShippingZoneRecord;
            }
            else if (country != null && country.Enabled && country.ShippingZoneRecord != null) {
                Cart.Properties["ShippingZone"] = country.ShippingZoneRecord;
            }
        }
    }
}