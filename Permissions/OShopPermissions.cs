using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Permissions {
    public class OShopPermissions : IPermissionProvider {
        public static readonly Permission ManageShopSettings = new Permission { Description = "Manage Shop Settings", Name = "ManageShopSettings" };

        public static readonly Permission AccessShopPanel = new Permission {
            Description = "Access Shop Panel",
            Name = "AccessShopPanel",
            ImpliedBy = new[] {
                OShopPermissions.ManageShopSettings
            }
        };

        public Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                OShopPermissions.AccessShopPanel,
                OShopPermissions.ManageShopSettings
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {
                        OShopPermissions.ManageShopSettings
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