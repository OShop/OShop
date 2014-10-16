using Orchard;
using OShop.Models;
using System;

namespace OShop.Services {
    interface ICustomersService : IDependency {
        CustomerPart GetCustomer();
        CustomerPart GetCustomer(Int32 UserId);
    }
}
