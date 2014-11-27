using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Customers")]
    public class CustomerAddressResolver : IShoppingCartBuilder, IOrderBuilder {
        private readonly ICustomersService _customersService;
        private readonly ILocationsService _locationsService;

        public CustomerAddressResolver (
            ICustomersService customersService,
            ILocationsService locationsService) {
            _customersService = customersService;
            _locationsService = locationsService;
        }

        public int Priority {
            get { return 950; }
        }

        public void BuildCart(IShoppingCartService ShoppingCartService, ref ShoppingCart Cart) {
            Int32 billingAddressId = ShoppingCartService.GetProperty<int>("BillingAddressId");
            Int32 shippingAddressId = ShoppingCartService.GetProperty<int>("ShippingAddressId");

            var addresses = _customersService.GetAddresses();

            if (billingAddressId > 0) {
                var billingAddress = addresses.Where(a => a.Id == billingAddressId).FirstOrDefault();
                if (billingAddress != null) {
                    Cart.BillingAddress = billingAddress;
                }
            }

            if (shippingAddressId > 0) {
                var shippingAddress = addresses.Where(a => a.Id == shippingAddressId).FirstOrDefault();
                if (shippingAddress != null) {
                    Cart.ShippingAddress = shippingAddress;
                }
            }
        }

        public void BuildOrder(IShoppingCartService ShoppingCartService, ref IContent Order) {
            var addresses = _customersService.GetAddresses();

            var orderPart = Order.As<OrderPart>();
            if (orderPart != null) {
                //  CustomerInfos
                var customer = _customersService.GetCustomer();
                var customerInfos = orderPart.CustomerInfos ?? new CustomerInfos();
                customerInfos.FirstName = customer.FirstName;
                customerInfos.LastName = customer.LastName;
                customerInfos.EMail = customer.Email;
                orderPart.CustomerInfos = customerInfos;

                //  Billing address
                Int32 billingAddressId = ShoppingCartService.GetProperty<int>("BillingAddressId");
                if (billingAddressId > 0) {
                    var billingAddress = addresses.Where(a => a.Id == billingAddressId).FirstOrDefault();
                    if (billingAddress != null) {
                        orderPart.BillingAddress = _locationsService.FormatAddress(billingAddress);
                    }
                }
            }

            var shippingPart = Order.As<OrderShippingPart>();
            if (shippingPart != null) {
                //  Shipping address
                Int32 shippingAddressId = ShoppingCartService.GetProperty<int>("ShippingAddressId");
                if (shippingAddressId > 0) {
                    var shippingAddress = addresses.Where(a => a.Id == shippingAddressId).FirstOrDefault();
                    if (shippingAddress != null) {
                        shippingPart.ShippingAddress = _locationsService.FormatAddress(shippingAddress);
                    }
                }
            }
        }

    }
}