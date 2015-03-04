using Orchard;
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
        private readonly IWorkContextAccessor _workContextAccessor;

        public CustomerAddressResolver (
            ICustomersService customersService,
            ILocationsService locationsService,
            IWorkContextAccessor workContextAccessor) {
            _customersService = customersService;
            _locationsService = locationsService;
            _workContextAccessor = workContextAccessor;
        }

        public int Priority {
            get { return 800; }
        }

        public void BuildCart(IShoppingCartService ShoppingCartService, ref ShoppingCart Cart) {
            Int32 billingAddressId = ShoppingCartService.GetProperty<int>("BillingAddressId");
            Int32 shippingAddressId = ShoppingCartService.GetProperty<int>("ShippingAddressId");

            var addresses = _customersService.GetMyAddresses();

            if (billingAddressId > 0) {
                var billingAddress = addresses.Where(a => a.Id == billingAddressId).FirstOrDefault();
                if (billingAddress != null) {
                    Cart.Properties["BillingAddress"] = billingAddress;
                }
            }

            if (shippingAddressId > 0) {
                var shippingAddress = addresses.Where(a => a.Id == shippingAddressId).FirstOrDefault();
                if (shippingAddress != null) {
                    // Set address
                    Cart.Properties["ShippingAddress"] = shippingAddress;

                    // Set shipping zone
                    var state = _locationsService.GetState(shippingAddress.StateId);
                    var country = _locationsService.GetCountry(shippingAddress.CountryId);
                    if (state != null && state.Enabled && state.ShippingZoneRecord != null) {
                        Cart.Properties["ShippingZone"] = state.ShippingZoneRecord;
                    }
                    else if (country != null && country.Enabled && country.ShippingZoneRecord != null) {
                        Cart.Properties["ShippingZone"] = country.ShippingZoneRecord;
                    }
                }
            }
        }

        public void BuildOrder(IShoppingCartService ShoppingCartService, ref IContent Order) {
            var addresses = _customersService.GetMyAddresses();

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
                        // Set address
                        shippingPart.ShippingAddress = _locationsService.FormatAddress(shippingAddress);

                        // Set shipping zone
                        var workContext = _workContextAccessor.GetContext();
                        var state = _locationsService.GetState(shippingAddress.StateId);
                        var country = _locationsService.GetCountry(shippingAddress.CountryId);
                        if (state != null && state.Enabled && state.ShippingZoneRecord != null) {
                            workContext.SetState("OShop.Orders.ShippingZone", state.ShippingZoneRecord);
                        }
                        else if (country != null && country.Enabled && country.ShippingZoneRecord != null) {
                            workContext.SetState("OShop.Orders.ShippingZone", country.ShippingZoneRecord);
                        }
                    }
                }
            }
        }

    }
}