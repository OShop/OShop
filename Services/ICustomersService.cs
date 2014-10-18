using Orchard;
using OShop.Models;
using System;

namespace OShop.Services {
    public interface ICustomersService : IDependency {
        CustomerPart GetCustomer();
        CustomerPart GetCustomer(Int32 UserId);
    }
}
