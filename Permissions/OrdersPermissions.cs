using Orchard.Environment.Extensions;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Permissions {
    [OrchardFeature("OShop.Orders")]
    public class OrdersPermissions : IPermissionProvider {
        public static readonly Permission ManageOrders = new Permission {
            Description = "Manage orders",
            Name = "ManageOrders",
            ImpliedBy = new[] {
                OShopPermissions.ManageShopSettings
            }
        };

        public static readonly Permission ViewOrders = new Permission {
            Description = "View orders",
            Name = "ViewOrders",
            ImpliedBy = new[] {
                OrdersPermissions.ManageOrders
            }
        };

        public static readonly Permission CreateOrders = new Permission {
            Description = "Create orders",
            Name = "CreateOrders",
            ImpliedBy = new[] {
                OrdersPermissions.ManageOrders
            }
        };

        public static readonly Permission ViewOwnOrders = new Permission {
            Description = "View own customer orders",
            Name = "ViewOwnOrders",
            ImpliedBy = new[] {
                OrdersPermissions.ManageOrders,
                OrdersPermissions.ViewOrders
            }
        };

        public Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                OrdersPermissions.ManageOrders,
                OrdersPermissions.ViewOrders,
                OrdersPermissions.CreateOrders,
                OrdersPermissions.ViewOwnOrders
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {
                        OrdersPermissions.ManageOrders
                    }
                },
                new PermissionStereotype {
                    Name = "Editor",
                    Permissions = new[] {
                        OrdersPermissions.ManageOrders
                    }
                }
            };
        }
    }
}