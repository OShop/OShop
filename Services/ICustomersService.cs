using Orchard;
using OShop.Models;
using System;
using System.Collections.Generic;

namespace OShop.Services {
    public interface ICustomersService : IDependency {
        CustomerPart GetCustomer();
        CustomerPart GetCustomer(Int32 UserId);
        IEnumerable<CustomerAddressPart> GetMyAddresses();
        IEnumerable<CustomerAddressPart> GetAddressesByOwner(Int32 UserId);
    }
}
