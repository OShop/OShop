using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Linq;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Locations")]
    public class LocationResolver : IShoppingCartResolver {
        private readonly ILocationsService _locationService;
        private readonly IShoppingCartService _shoppingCartService;

        public LocationResolver(
            ILocationsService locationService,
            IShoppingCartService shoppingCartService) {
            _locationService = locationService;
            _shoppingCartService = shoppingCartService;
        }

        public Int32 Priority {
            get { return 1000; }
        }

        public void ResolveCart(ref ShoppingCart Cart) {
            Int32 countryId = _shoppingCartService.GetProperty<int>("CountryId");
            Int32 stateId = _shoppingCartService.GetProperty<int>("StateId");

            var country = _locationService.GetCountry(countryId);

            if (country != null && country.Enabled) {
                var state = country.States.Where(s => s.Id == stateId && s.Enabled).FirstOrDefault();
                Cart.Country = country;
                Cart.State = state;
            }
            else {
                Cart.Country = _locationService.GetDefaultCountry();
                Cart.State = null;
            }
        }
    }
}