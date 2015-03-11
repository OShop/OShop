using Orchard;
using OShop.Models;
using System;
using System.Collections.Generic;

namespace OShop.Services {
    public interface ICustomersService : IDependency {
        CustomerPart GetCustomer();
        CustomerPart GetCustomer(Int32 CustomerId);
        IEnumerable<CustomerAddressPart> GetAddressesForCustomer(CustomerPart Customer);
        IEnumerable<CustomerAddressPart> GetAddressesForCustomer(Int32 CustomerId);
    }
}
