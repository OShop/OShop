using Orchard.Environment.Extensions;
using OShop.Helpers;
using OShop.Models;
using System;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingZoneResolver : IShoppingCartBuilder {
        private readonly ILocationsService _locationsService;

        public ShippingZoneResolver(
            ILocationsService locationsService) {
            _locationsService = locationsService;
        }

        public Int32 Priority {
            get { return 900; }
        }

        public void BuildCart(IShoppingCartService ShoppingCartService, ref ShoppingCart Cart) {
            LocationsStateRecord state = null;
            LocationsCountryRecord country = null;

            // Based on user selected location
            Int32 countryId = ShoppingCartService.GetProperty<int>("CountryId");
            if (countryId > 0) {
                country = _locationsService.GetCountry(countryId);
                Int32 stateId = ShoppingCartService.GetProperty<int>("StateId");
                if (stateId > 0) {
                    state = _locationsService.GetState(stateId);
                }
            }

            if (state != null && state.Enabled && state.ShippingZoneRecord != null) {
                Cart.Properties["ShippingZone"] = state.ShippingZoneRecord;
            }
            else if (country != null && country.Enabled && country.ShippingZoneRecord != null) {
                Cart.Properties["ShippingZone"] = country.ShippingZoneRecord;
            }
        }
    }
}