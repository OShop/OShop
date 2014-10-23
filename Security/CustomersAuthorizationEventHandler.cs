using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Environment.Extensions;
using Orchard.Security;
using OShop.Models;
using OShop.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Security {
    [OrchardFeature("OShop.Customers")]
    public class CustomersAuthorizationEventHandler : IAuthorizationServiceEventHandler {
        public void Checking(CheckAccessContext context) {
            // Override permissions for Customer account infos
            if (context.Content.Is<CustomerPart>() || context.Content.Is<CustomerAddressPart>()) {
                if (context.Permission.Name == Orchard.Core.Contents.Permissions.PublishContent.Name) {
                    context.Permission = HasOwnership(context) ? CustomersPermissions.EditOwnCustomerAccount : CustomersPermissions.ManageCustomerAccounts;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.PublishOwnContent.Name) {
                    context.Permission = CustomersPermissions.EditOwnCustomerAccount;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.EditContent.Name) {
                    context.Permission = HasOwnership(context) ? CustomersPermissions.EditOwnCustomerAccount : CustomersPermissions.ManageCustomerAccounts;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.EditOwnContent.Name) {
                    context.Permission = CustomersPermissions.EditOwnCustomerAccount;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.DeleteContent.Name) {
                    context.Permission = HasOwnership(context) ? CustomersPermissions.EditOwnCustomerAccount : CustomersPermissions.ManageCustomerAccounts;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.DeleteOwnContent.Name) {
                    context.Permission = CustomersPermissions.EditOwnCustomerAccount;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.ViewContent.Name) {
                    context.Permission = HasOwnership(context) ? CustomersPermissions.ViewOwnCustomerAccount : CustomersPermissions.ViewCustomerAccounts;
                }
                else if (context.Permission.Name == Orchard.Core.Contents.Permissions.ViewOwnContent.Name) {
                    context.Permission = CustomersPermissions.ViewOwnCustomerAccount;
                }
            }
        }

        public void Adjust(CheckAccessContext context) {
        }

        public void Complete(CheckAccessContext context) { }

        private static bool HasOwnership(CheckAccessContext context) {
            if (context.User == null || context.Content == null)
                return false;

            var common = context.Content.As<ICommonPart>();
            if (common == null || common.Owner == null)
                return false;

            return context.User.Id == common.Owner.Id;
        }

    }
}