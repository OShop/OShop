using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Environment.Extensions;
using Orchard.Security;
using OShop.Models;
using OShop.Permissions;

namespace OShop.Security {
    [OrchardFeature("OShop.Orders")]
    public class OrdersAuthorizationEventHandler : IAuthorizationServiceEventHandler {
        public void Checking(CheckAccessContext context) { }

        public void Adjust(CheckAccessContext context) {
            if (context.Content.Is<OrderPart>()) {
                if (context.Permission.Name == Orchard.Core.Contents.Permissions.PublishContent.Name || context.Permission.Name == Orchard.Core.Contents.Permissions.PublishOwnContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = OrdersPermissions.CreateOrders;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.EditContent.Name || context.Permission.Name == Orchard.Core.Contents.Permissions.EditOwnContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = OrdersPermissions.ManageOrders;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.DeleteContent.Name || context.Permission.Name == Orchard.Core.Contents.Permissions.DeleteOwnContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = OrdersPermissions.ManageOrders;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.ViewContent.Name || context.Permission.Name == Orchard.Core.Contents.Permissions.ViewOwnContent.Name) {
                    context.Granted = false;
                    context.Adjusted = true;
                    context.Permission = OrdersPermissions.ViewOrders;
                }
            }
        }

        public void Complete(CheckAccessContext context) { }

    }
}