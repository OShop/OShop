using Orchard.Environment.Extensions;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Permissions {
    [OrchardFeature("OShop.Payment")]
    public class PaymentPermissions : IPermissionProvider {
        public static readonly Permission ManagePayments = new Permission { Description = "Manage payments", Name = "ManagePayments" };

        public Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                PaymentPermissions.ManagePayments
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {
                        PaymentPermissions.ManagePayments
                    }
                }
            };
        }
    }
}