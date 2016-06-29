using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;
using OShop.Services;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Customers")]
    public class CustomerAddressPartHandler : ContentHandler {
        private ICustomersService _customersService;
        private ILocationsService _locationService;

        public CustomerAddressPartHandler(
            ICustomersService customersService,
            ILocationsService locationService,
            IRepository<CustomerAddressPartRecord> repository
            ) {
            _customersService = customersService;
            _locationService = locationService;

            Filters.Add(StorageFilter.For(repository));

            OnActivated<CustomerAddressPart>((context, part) => {
                part._customer.Loader(() => _customersService.GetCustomer(part.CustomerId));
                part._customer.Setter(customer => {
                    part.CustomerId = customer != null ? customer.Id : 0;
                    return customer;
                });

                part._country.Loader(() => _locationService.GetCountry(part.CountryId));
                part._country.Setter(country => {
                    part.CountryId = country != null ? country.Id : 0;
                    return country;
                });

                part._state.Loader(() => _locationService.GetState(part.StateId));
                part._state.Setter(state => {
                    part.StateId = state != null ? state.Id : 0;
                    return state;
                });
            });
        }
    }
}