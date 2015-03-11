using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Environment.Extensions;
using Orchard.Security;
using OShop.Models;
using OShop.Permissions;

namespace OShop.Security {
    [OrchardFeature("OShop.Customers")]
    public class CustomersAuthorizationEventHandler : IAuthorizationServiceEventHandler {
        public void Checking(CheckAccessContext context) { }

        public void Adjust(CheckAccessContext context) {
            if (context.Content.Is<CustomerPart>() || context.Content.Is<CustomerAddressPart>()) {

                if (context.Permission.Name == Orchard.Core.Contents.Permissions.PublishContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = HasOwnership(context.User, context.Content) ? CustomersPermissions.EditOwnCustomerAccount : CustomersPermissions.ManageCustomerAccounts;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.PublishOwnContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = CustomersPermissions.EditOwnCustomerAccount;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.EditContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = HasOwnership(context.User, context.Content) ? CustomersPermissions.EditOwnCustomerAccount : CustomersPermissions.ManageCustomerAccounts;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.EditOwnContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = CustomersPermissions.EditOwnCustomerAccount;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.DeleteContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = HasOwnership(context.User, context.Content) ? CustomersPermissions.EditOwnCustomerAccount : CustomersPermissions.ManageCustomerAccounts;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.DeleteOwnContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = CustomersPermissions.EditOwnCustomerAccount;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.ViewContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = HasOwnership(context.User, context.Content) ? CustomersPermissions.ViewOwnCustomerAccount : CustomersPermissions.ViewCustomerAccounts;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.ViewOwnContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = CustomersPermissions.ViewOwnCustomerAccount;
                }
            }
        }

        public void Complete(CheckAccessContext context) { }

        private static bool HasOwnership(IUser user, IContent content) {
            if (user == null || content == null)
                return false;
            CustomerPart customer;
            customer = content.As<CustomerPart>();
            if (customer == null) {
                var address = content.As<CustomerAddressPart>();
                if (address != null) {
                    customer = address.Customer;
                }
            }

            return customer != null && user.Id == customer.UserId;
        }
    }
}