using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Security;
using OShop.Models;
using OShop.Services;
using System.Web.Routing;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Customers")]
    public class CustomerPartHandler : ContentHandler {
        public CustomerPartHandler(
            IContentManager contentManager,
            ICustomersService customersService,
            IRepository<CustomerPartRecord> repository
            ) {
            Filters.Add(StorageFilter.For(repository));

            OnActivated<CustomerPart>((context, part) => {
                // User field
                part._user.Loader(() => contentManager.Get<IUser>(part.UserId));
                part._user.Setter(user => {
                    part.UserId = (user != null ? user.Id : 0);
                    return user;
                });

                // Default address field
                part._defaultAddress.Loader(() => customersService.GetAddress(part.DefaultAddressId));
                part._defaultAddress.Setter(address => {
                    part.DefaultAddressId = (address != null ? address.Id : 0);
                    return address;
                });

                // Addresses field
                part._addresses.Loader(() => customersService.GetAddressesForCustomer(part));
            });

            OnRemoving<CustomerPart>((context, part) => {
                foreach (var address in part.Addresses) {
                    contentManager.Remove(address.ContentItem);
                }
            });
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            var customer = context.ContentItem.As<CustomerPart>();

            if (customer == null)
                return;

            // Admin link shows customer details
            context.Metadata.AdminRouteValues = new RouteValueDictionary {
                {"Area", "OShop"},
                {"Controller", "CustomersAdmin"},
                {"Action", "Detail"},
                {"Id", customer.Id}
            };
        }
    }
}