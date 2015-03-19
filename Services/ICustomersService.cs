using Orchard;
using OShop.Models;
using System;
using System.Collections.Generic;

namespace OShop.Services {
    public interface ICustomersService : IDependency {
        CustomerPart GetCustomer();
        CustomerPart GetCustomer(Int32 CustomerId, Int32? VersionRecordId = null);
        IEnumerable<CustomerAddressPart> GetAddressesForCustomer(CustomerPart Customer);
        IEnumerable<CustomerAddressPart> GetAddressesForCustomer(Int32 CustomerId);
        CustomerAddressPart GetAddress(Int32 CustomerAddressId, Int32? VersionRecordId = null);
    }
}
