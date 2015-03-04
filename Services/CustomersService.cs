using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Environment.Extensions;
using Orchard.Security;
using Orchard.Services;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

            return GetCustomer(user.Id);
        }

        public CustomerPart GetCustomer(int UserId) {
            var commonPart =  _contentManager.Query<CommonPart, CommonPartRecord>()
                .Where(c => c.OwnerId == UserId).Slice(1)
                .FirstOrDefault();

            if (commonPart != null) {
                return commonPart.ContentItem.As<CustomerPart>();
            }
            else {
                return null;
            }
        }

        public IEnumerable<CustomerAddressPart> GetMyAddresses() {
            var user = _authenticationService.GetAuthenticatedUser();

            if (user == null) {
                return new List<CustomerAddressPart>();
            }

            return GetAddressesByOwner(user.Id);
        }

        public IEnumerable<CustomerAddressPart> GetAddressesByOwner(int UserId) {
            return _contentManager.Query<CommonPart, CommonPartRecord>()
                .Where(c => c.OwnerId == UserId)
                .ForPart<CustomerAddressPart>()
                .List();
        }
    }
}