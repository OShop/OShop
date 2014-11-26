using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Customers")]
    public class CustomerAddressResolver : IShoppingCartBuilder {
        private readonly ICustomersService _customersService;

        public CustomerAddressResolver (
            ICustomersService customersService) {
            _customersService = customersService;
        }

        public int Priority {
            get { return 950; }
        }

        public void BuildCart(IShoppingCartService ShoppingCartService, ref ShoppingCart Cart) {
            Int32 billingAddressId = ShoppingCartService.GetProperty<int>("BillingAddressId");
            Int32 shippingAddressId = ShoppingCartService.GetProperty<int>("ShippingAddressId");

            if (billingAddressId > 0) {
                var billingAddress = _customersService.GetAddresses().Where(a => a.Id == billingAddressId).FirstOrDefault();
                if (billingAddress != null) {
                    Cart.BillingAddress = billingAddress;
                }
            }

            if (shippingAddressId > 0) {
                var shippingAddress = _customersService.GetAddresses().Where(a => a.Id == shippingAddressId).FirstOrDefault();
                if (shippingAddress != null) {
                    Cart.ShippingAddress = shippingAddress;
                }
            }
        }
    }
}