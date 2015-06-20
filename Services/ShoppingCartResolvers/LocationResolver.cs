using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using OShop.Extensions;
using OShop.Models;
using System;
using System.Web.Mvc;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Locations")]
    public class LocationResolver : IShoppingCartBuilder {
        private readonly ILocationsService _locationsService;

        public LocationResolver(
            ILocationsService locationsService) {
            _locationsService = locationsService;
        }

        public Int32 Priority {
            get { return 1000; }
        }

        public void BuildCart(IShoppingCartService ShoppingCartService, ShoppingCart Cart) {
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
            else {
                // Set default country
                country = _locationsService.GetDefaultCountry();
                if (country != null) {
                    ShoppingCartService.SetProperty<int>("CountryId", country.Id);
                }
            }

            Cart.Properties["BillingCountry"] = country;
            Cart.Properties["BillingState"] = state;
            Cart.Properties["ShippingCountry"] = country;
            Cart.Properties["ShippingState"] = state;
        }
    }
}