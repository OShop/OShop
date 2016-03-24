using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Themes;
using Orchard.UI.Notify;
using OShop.Extensions;
using OShop.Models;
using OShop.Services;
using OShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [OrchardFeature("OShop.Checkout")]
    public class CheckoutController : Controller
    {
        private readonly ICustomersService _customersService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrdersService _ordersService;
        private readonly IShippingService _shippingService;
        private readonly IEnumerable<IPaymentProvider> _paymentProviders;
        private readonly dynamic _shapeFactory;

        public CheckoutController(
            ICustomersService customersService,
            IShoppingCartService shoppingCartService,
            IOrdersService ordersService,
            IOrchardServices services,
            IShapeFactory shapeFactory,
            IEnumerable<IPaymentProvider> paymentProviders,
            IShippingService shippingService = null) {
            _customersService = customersService;
            _shoppingCartService = shoppingCartService;
            _ordersService = ordersService;
            _shapeFactory = shapeFactory;
            _shippingService = shippingService;
            _paymentProviders = paymentProviders.OrderByDescending(p => p.Priority);
            Services = services;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }

        [Themed]
        public ActionResult Index() {
            if(Services.WorkContext.CurrentUser == null) {
                return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
            }

            var customer = _customersService.GetCustomer();
            if(customer == null) {
                return RedirectToAction("Create", "Customer", new { area = "OShop", ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
            }

            if (!_shoppingCartService.ListItems().Any()) {
                // Cart is empty => Return to Shopping cart
                return RedirectToAction("Index", "ShoppingCart", new { area = "OShop" });
            }

            // Set flag for CustomerResolver
            _shoppingCartService.SetProperty<String>("Checkout", "Checkout");

            ShoppingCart cart = _shoppingCartService.BuildCart();

            var checkoutShape = _shapeFactory.Checkout()
                .Cart(cart)
                .Addresses(customer.Addresses);

            if (_shippingService != null && cart.IsShippingRequired()) {
                // Shipping option selection
                checkoutShape.ShippingOptions = _shapeFactory.ShoppingCart_ShippingOptions()
                    .Cart(cart)
                    .ContentItems(_shapeFactory.List()
                        .AddRange(_shippingService.GetSuitableProviderOptions(
                                cart.Properties["ShippingZone"] as ShippingZoneRecord,
                                cart.Properties["ShippingInfos"] as IList<Tuple<int, IShippingInfo>> ?? new List<Tuple<int, IShippingInfo>>(),
                                cart.ItemsTotal()
                            ).OrderBy(p => p.Option.Price)
                            .Select(sp => _shapeFactory.ShoppingCart_ShippingOption()
                                .Cart(cart)
                                .ProviderOption(sp)
                            )
                        )
                    );
            }

            return new ShapeResult(this, checkoutShape);
        }

        [Themed]
        [Authorize]
        [HttpPost, ActionName("Index")]
        public ActionResult IndexPost(string Action, CheckoutIndexViewModel Model) {
            _shoppingCartService.SetProperty("BillingAddressId", Model.BillingAddressId);
            _shoppingCartService.SetProperty("ShippingAddressId", Model.ShippingAddressId);
            _shoppingCartService.SetProperty("ShippingProviderId", Model.ShippingProviderId);

            switch (Action) {
                case "EditShippingAddress":
                    return RedirectToAction("EditAddress", "Customer", new { area = "OShop", id = Model.ShippingAddressId, ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
                case "RemoveShippingAddress":
                    _shoppingCartService.RemoveProperty("ShippingAddressId");
                    return RedirectToAction("RemoveAddress", "Customer", new { area = "OShop", id = Model.ShippingAddressId, ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
                case "EditBillingAddress":
                    return RedirectToAction("EditAddress", "Customer", new { area = "OShop", id = Model.BillingAddressId, ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
                case "RemoveBillingAddress":
                    _shoppingCartService.RemoveProperty("RemoveBillingAddress");
                    return RedirectToAction("RemoveAddress", "Customer", new { area = "OShop", id = Model.BillingAddressId, ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
                case "Validate":
                    return ValidateAddress();
                default:
                    return Index();
            }
        }

        [Themed]
        [Authorize]
        public ActionResult ValidateOrder() {
            var order = _shoppingCartService.BuildOrder();
            TempData["OShop.Checkout.Order"] = order;

            return new ShapeResult(this, _shapeFactory.Checkout_Validate(Order: order, PaymentProviders: _paymentProviders));
        }

        [Themed]
        [Authorize]
        [HttpPost, ActionName("ValidateOrder")]
        public ActionResult ValidateOrderPost(string Payment) {
            var order = TempData["OShop.Checkout.Order"] as IContent;
            if (order != null) {
                _ordersService.CreateOrder(order);
                _shoppingCartService.Empty();

                var paymentPart = order.As<PaymentPart>();
                if (paymentPart != null && !String.IsNullOrWhiteSpace(Payment)) {
                    var paymentProvider = _paymentProviders.Where(p => p.Name == Payment).FirstOrDefault();
                    if (paymentProvider != null) {
                        var paymentRoute = paymentProvider.GetPaymentRoute(paymentPart);
                        if (paymentRoute != null) {
                            return RedirectToRoute(paymentRoute);
                        }
                    }
                }

                return RedirectToAction("Detail", "Orders", new { area = "OShop", id = order.As<OrderPart>().Reference });
            }
            else {
                return ValidateOrder();
            }
        }

        private ActionResult ValidateAddress() {
            var cart = _shoppingCartService.BuildCart();
            Boolean isValid = cart.IsValid;

            if (cart.Properties["BillingAddress"] as IOrderAddress == null) {
                isValid = false;
                Services.Notifier.Error(T("Please provide your billing address."));
            }

            if (cart.IsShippingRequired()) {
                if (cart.Properties["ShippingAddress"] == null) {
                    isValid = false;
                    Services.Notifier.Error(T("Please provide your shipping address."));
                }
                if (cart.Shipping as ShippingProviderOption == null) {
                    isValid = false;
                    Services.Notifier.Error(T("Please select a shipping method."));
                }
            }

            if (!isValid) {
                return Index();
            }
            else {
                return RedirectToAction("ValidateOrder");
            }
        }

    }
}