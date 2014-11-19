using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Linq;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Locations")]
    public class LocationResolver : IShoppingCartResolver {
        private readonly ILocationsService _locationService;

        public LocationResolver(
            ILocationsService locationService) {
            _locationService = locationService;
        }

        public Int32 Priority {
            get { return 1000; }
        }

        public void ResolveCart(IShoppingCartService ShoppingCartService, ref ShoppingCart Cart) {
            Int32 countryId = ShoppingCartService.GetProperty<int>("CountryId");
            Int32 stateId = ShoppingCartService.GetProperty<int>("StateId");

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