using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using OShop.Helpers;
using OShop.Models;
using System;
using System.Linq;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Checkout")]
    public class CustomerResolver : IShoppingCartBuilder, IOrderBuilder {
        private readonly ICustomersService _customersService;
        private readonly IWorkContextAccessor _workContextAccessor;

        public CustomerResolver (
            ICustomersService customersService,
            IWorkContextAccessor workContextAccessor) {
            _customersService = customersService;
            _workContextAccessor = workContextAccessor;
        }

        public int Priority {
            get { return 900; }
        }

        public void BuildCart(IShoppingCartService ShoppingCartService, ShoppingCart Cart) {
            var customer = _customersService.GetCustomer();

            if (customer == null) {
                return;
            }

            // Get "Checkout" property to know the Checkout provider beeing used
            var checkout = ShoppingCartService.GetProperty<string>("Checkout");

            if (string.IsNullOrWhiteSpace(checkout) && customer.DefaultAddress != null) {
                // Override default location
                ShoppingCartService.SetProperty<int>("CountryId", customer.DefaultAddress.CountryId);
                ShoppingCartService.SetProperty<int>("StateId", customer.DefaultAddress.StateId);

                Cart.Properties["BillingCountry"] = customer.DefaultAddress.Country;
                Cart.Properties["BillingState"] = customer.DefaultAddress.State;
                Cart.Properties["ShippingCountry"] = customer.DefaultAddress.Country;
                Cart.Properties["ShippingState"] = customer.DefaultAddress.State;
            }
            else if (checkout == "Checkout") {
                var billingAddress = customer.Addresses.Where(a => a.Id == ShoppingCartService.GetProperty<int>("BillingAddressId")).FirstOrDefault() ?? customer.DefaultAddress ?? customer.Addresses.FirstOrDefault();
                var shippingAddress = customer.Addresses.Where(a => a.Id == ShoppingCartService.GetProperty<int>("ShippingAddressId")).FirstOrDefault() ?? customer.DefaultAddress ?? customer.Addresses.FirstOrDefault();

                if (billingAddress != null) {
                    Cart.Properties["BillingAddress"] = billingAddress;
                    Cart.Properties["BillingCountry"] = billingAddress.Country;
                    Cart.Properties["BillingState"] = billingAddress.State;
                }

                if (shippingAddress != null) {
                    Cart.Properties["ShippingAddress"] = shippingAddress;
                    Cart.Properties["ShippingCountry"] = shippingAddress.Country;
                    Cart.Properties["ShippingState"] = shippingAddress.State;
                }
            }
        }

        public void BuildOrder(IShoppingCartService ShoppingCartService, IContent Order) {
            var customer = _customersService.GetCustomer();

            if (customer == null) {
                return;
            }

            OrderAddressRecord billingAddress = null, shippingAddress = null;
            Int32 billingAddressId = ShoppingCartService.GetProperty<int>("BillingAddressId");
            if (billingAddressId > 0) {
                var customerBillingAddress = customer.Addresses.Where(a => a.Id == billingAddressId).FirstOrDefault();
                if (customerBillingAddress != null) {
                    billingAddress = new OrderAddressRecord();
                    customerBillingAddress.CopyTo(billingAddress);
                }
            }
            Int32 shippingAddressId = ShoppingCartService.GetProperty<int>("ShippingAddressId");
            if (shippingAddressId > 0) {
                if (shippingAddressId == billingAddressId) {
                    shippingAddress = billingAddress;
                }
                else {
                    var customerShippingAddress = customer.Addresses.Where(a => a.Id == shippingAddressId).FirstOrDefault();
                    if (customerShippingAddress != null) {
                        shippingAddress = new OrderAddressRecord();
                        customerShippingAddress.CopyTo(shippingAddress);
                    }
                }
            }

            var customerOrderPart = Order.As<CustomerOrderPart>();
            if (customerOrderPart != null) {
                customerOrderPart.Customer = customer;
            }

            var orderPart = Order.As<OrderPart>();
            if (orderPart != null && billingAddress != null) {
                orderPart.BillingAddress = billingAddress;
            }

            var shippingPart = Order.As<OrderShippingPart>();
            if (shippingPart != null) {
                //  Shipping address
                if (shippingAddress != null) {
                    // Set address
                    shippingPart.ShippingAddress = shippingAddress;

                    // Set shipping zone
                    var workContext = _workContextAccessor.GetContext();
                    if (shippingAddress.State != null && shippingAddress.State.Enabled && shippingAddress.State.ShippingZoneRecord != null) {
                        workContext.SetState("OShop.Orders.ShippingZone", shippingAddress.State.ShippingZoneRecord);
                    }
                    else if (shippingAddress.Country != null && shippingAddress.Country.Enabled && shippingAddress.Country.ShippingZoneRecord != null) {
                        workContext.SetState("OShop.Orders.ShippingZone", shippingAddress.Country.ShippingZoneRecord);
                    }
                }
            }
        }

    }
}