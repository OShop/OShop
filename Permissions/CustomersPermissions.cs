using Orchard.Environment.Extensions;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Permissions {
    [OrchardFeature("OShop.Customers")]
    public class CustomersPermissions : IPermissionProvider {
        public static readonly Permission ManageCustomers = new Permission {
            Description = "Manage customer accounts",
            Name = "ManageCustomers",
            ImpliedBy = new[] {
            OShopPermissions.ManageShopSettings
        }
        };

        public Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                CustomersPermissions.ManageCustomers
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {
                        CustomersPermissions.ManageCustomers
                    }
                },
                new PermissionStereotype {
                    Name = "Editor",
                    //Permissions = new[] {}
                }
            };
        }
    }
}