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

            var dummyAddress = new OrderAddress();

            if (country != null && country.Enabled) {
                var state = country.States.Where(s => s.Id == stateId && s.Enabled).FirstOrDefault();
                dummyAddress.CountryId = country != null ? country.Id : 0;
                dummyAddress.StateId = state != null ? state.Id : 0;
            }
            else {
                country = _locationService.GetDefaultCountry();
                dummyAddress.CountryId = country != null ? country.Id : 0;
                dummyAddress.StateId = 0;
            }

            Cart.ShippingAddress = dummyAddress;
            Cart.BillingAddress = dummyAddress;
        }
    }
}