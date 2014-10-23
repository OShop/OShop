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
        public static readonly Permission ManageCustomerAccounts = new Permission {
            Description = "Manage customer accounts",
            Name = "ManageCustomerAccounts",
            ImpliedBy = new[] {
                OShopPermissions.ManageShopSettings
            }
        };

        public static readonly Permission ViewCustomerAccounts = new Permission {
            Description = "View customer accounts",
            Name = "ViewCustomerAccounts",
            ImpliedBy = new[] {
                CustomersPermissions.ManageCustomerAccounts
            }
        };

        public static readonly Permission EditOwnCustomerAccount = new Permission {
            Description = "Edit own customer accounts",
            Name = "EditOwnCustomerAccount",
            ImpliedBy = new[] {
                CustomersPermissions.ManageCustomerAccounts
            }
        };

        public static readonly Permission ViewOwnCustomerAccount = new Permission {
            Description = "View own customer accounts",
            Name = "ViewOwnCustomerAccount",
            ImpliedBy = new[] {
                CustomersPermissions.ViewCustomerAccounts,
                CustomersPermissions.EditOwnCustomerAccount
            }
        };

        public Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                CustomersPermissions.ManageCustomerAccounts,
                CustomersPermissions.ViewCustomerAccounts,
                CustomersPermissions.EditOwnCustomerAccount,
                CustomersPermissions.ViewOwnCustomerAccount
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {
                        CustomersPermissions.ManageCustomerAccounts
                    }
                },
                new PermissionStereotype {
                    Name = "Editor",
                    Permissions = new[] {
                        CustomersPermissions.ViewCustomerAccounts
                    }
                },
                new PermissionStereotype {
                    Name = "Authenticated",
                    Permissions = new[] {
                        CustomersPermissions.ViewOwnCustomerAccount
                    }
                }
            };
        }
    }
}