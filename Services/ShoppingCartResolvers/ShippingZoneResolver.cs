using Orchard.Environment.Extensions;
using OShop.Helpers;
using OShop.Models;
using System;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingZoneResolver : IShoppingCartResolver {
        private readonly ILocationsService _locationService;

        public ShippingZoneResolver(
            ILocationsService locationService) {
            _locationService = locationService;
        }

        public Int32 Priority {
            get { return 990; }
        }

        public void ResolveCart(ref ShoppingCart Cart) {
            if (Cart.ShippingAddress != null) {
                var state = _locationService.GetState(Cart.ShippingAddress.StateId);
                var country = _locationService.GetCountry(Cart.ShippingAddress.CountryId);
                if (state != null && state.Enabled && state.ShippingZoneRecord != null) {
                    Cart.ShippingZone = state.ShippingZoneRecord;
                }
                else if (country != null && country.Enabled && country.ShippingZoneRecord != null) {
                    Cart.ShippingZone = country.ShippingZoneRecord;
                }
                else {
                    Cart.ShippingZone = null;
                }
            }
            else {
                Cart.ShippingZone = null;
            }
        }
    }
}