using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;
using OShop.Services;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Customers")]
    public class CustomerAddressPartHandler : ContentHandler {
        private ICustomersService _customersService;

        public CustomerAddressPartHandler(
            ICustomersService customersService,
            IRepository<CustomerAddressPartRecord> repository
            ) {
            _customersService = customersService;

            Filters.Add(StorageFilter.For(repository));

            OnActivated<CustomerAddressPart>((context, part) => {
                part._customer.Loader(customer => _customersService.GetCustomer(part.Record.CustomerId));
                part._customer.Setter(customer => {
                    part.Record.CustomerId = customer.ContentItem.Id;
                    return customer;
                });
            });
        }
    }
}