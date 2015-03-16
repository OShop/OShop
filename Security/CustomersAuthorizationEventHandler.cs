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
                if (context.Permission.Name == Orchard.Core.Contents.Permissions.PublishContent.Name || context.Permission.Name == Orchard.Core.Contents.Permissions.PublishOwnContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = HasOwnership(context.User, context.Content) ? CustomersPermissions.EditOwnCustomerAccount : CustomersPermissions.ManageCustomerAccounts;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.EditContent.Name || context.Permission.Name == Orchard.Core.Contents.Permissions.EditOwnContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = HasOwnership(context.User, context.Content) ? CustomersPermissions.EditOwnCustomerAccount : CustomersPermissions.ManageCustomerAccounts;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.DeleteContent.Name || context.Permission.Name == Orchard.Core.Contents.Permissions.DeleteOwnContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = HasOwnership(context.User, context.Content) ? CustomersPermissions.EditOwnCustomerAccount : CustomersPermissions.ManageCustomerAccounts;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.ViewContent.Name || context.Permission.Name == Orchard.Core.Contents.Permissions.ViewOwnContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = HasOwnership(context.User, context.Content) ? CustomersPermissions.ViewOwnCustomerAccount : CustomersPermissions.ViewCustomerAccounts;
                }
            }
            else if (context.Content.Is<CustomerOrderPart>()) {
                if (context.Permission.Name == Orchard.Core.Contents.Permissions.PublishContent.Name || context.Permission.Name == Orchard.Core.Contents.Permissions.PublishOwnContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = HasOwnership(context.User, context.Content) ? OrdersPermissions.CreateOrders : OrdersPermissions.ManageOrders;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.ViewContent.Name || context.Permission.Name == Orchard.Core.Contents.Permissions.ViewOwnContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = HasOwnership(context.User, context.Content) ? OrdersPermissions.ViewOwnOrders : OrdersPermissions.ViewOrders;
                }
                else if (!context.Granted && context.Permission.Name == OrdersPermissions.ViewOrders.Name && HasOwnership(context.User, context.Content)) {
                    context.Adjusted = true;
                    context.Permission = OrdersPermissions.ViewOwnOrders;
                }
            }
        }

        public void Complete(CheckAccessContext context) { }

        private static bool HasOwnership(IUser user, IContent content) {
            if (user == null || content == null)
                return false;

            if (content.Is<CustomerPart>()) {
                var customer = content.As<CustomerPart>();
                return customer != null && user.Id == customer.UserId;
            }
            else if (content.Is<CustomerAddressPart>()) {
                var address = content.As<CustomerAddressPart>();
                return address != null && address.Customer != null && user.Id == address.Customer.UserId;
            }
            else if (content.Is<CustomerOrderPart>()) {
                var order = content.As<CustomerOrderPart>();
                return order != null && order.Customer != null && user.Id == order.Customer.UserId;
            }

            return false;
        }
    }
}