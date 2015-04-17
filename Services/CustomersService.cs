using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Security;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OShop.Services {
    [OrchardFeature("OShop.Customers")]
    public class CustomersService : ICustomersService {
        private readonly IContentManager _contentManager;
        private readonly IAuthenticationService _authenticationService;

        public CustomersService(
            IContentManager contentManager,
            IAuthenticationService authenticationService
            ) {
            _contentManager = contentManager;
            _authenticationService = authenticationService;
        }

        public CustomerPart GetCustomer() {
            var user = _authenticationService.GetAuthenticatedUser();

            if (user == null) {
                return null;
            }

            return GetCustomerForUser(user.Id);
        }

        public CustomerPart GetCustomer(int CustomerId) {
            return _contentManager.Get<CustomerPart>(CustomerId, VersionOptions.Published);
        }

        private CustomerPart GetCustomerForUser(int UserId) {
            var customerPart = _contentManager.Query<CustomerPart, CustomerPartRecord>()
                .Where(c => c.UserId == UserId).Slice(1)
                .FirstOrDefault();

            if (customerPart != null) {
                return customerPart;
            }
            else {
                return null;
            }
        }

        public IEnumerable<CustomerAddressPart> GetAddressesForCustomer(CustomerPart Customer) {
            if (Customer == null) {
                return new List<CustomerAddressPart>();
            }

            return GetAddressesForCustomer(Customer.ContentItem.Id);
        }

        public IEnumerable<CustomerAddressPart> GetAddressesForCustomer(int CustomerId) {
            return _contentManager.Query<CustomerAddressPart, CustomerAddressPartRecord>()
                .Where(c => c.CustomerId == CustomerId)
                .List();
        }

        public CustomerAddressPart GetAddress(int CustomerAddressId) {
            return _contentManager.Get<CustomerAddressPart>(CustomerAddressId, VersionOptions.Published);
        }
    }
}