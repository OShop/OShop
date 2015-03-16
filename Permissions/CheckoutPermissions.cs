using Orchard.Environment.Extensions;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Permissions {
    [OrchardFeature("OShop.Checkout")]
    public class CheckoutPermissions : IPermissionProvider {
        public Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new Permission[0];
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Authenticated",
                    Permissions = new[] {
                        OrdersPermissions.CreateOrders
                    }
                }
            };
        }
    }
}