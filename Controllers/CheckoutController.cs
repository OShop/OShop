using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Themes;
using Orchard.UI.Notify;
using OShop.Helpers;
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
        private readonly ICurrencyProvider _currencyProvider;
        private readonly IContentManager _contentManager;
        private readonly IShippingService _shippingService;
        private readonly dynamic _shapeFactory;

        public CheckoutController(
            ICustomersService customersService,
            IShoppingCartService shoppingCartService,
            ICurrencyProvider currencyProvider,
            IContentManager contentManager,
            IOrchardServices services,
            IShapeFactory shapeFactory,
            IShippingService shippingService = null
            ) {
            _customersService = customersService;
            _shoppingCartService = shoppingCartService;
            _currencyProvider = currencyProvider;
            _contentManager = contentManager;
            _shapeFactory = shapeFactory;
            _shippingService = shippingService;
            Services = services;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }

        [Themed]
        [Authorize]
        public ActionResult Index()
        {
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
                                .NumberFormat(_currencyProvider.NumberFormat)
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

            return new ShapeResult(this, _shapeFactory.Checkout_Validate()
                .Order(order));
        }

        [Themed]
        [Authorize]
        [HttpPost, ActionName("ValidateOrder")]
        public ActionResult ValidateOrderPost() {
            var order = TempData["OShop.Checkout.Order"] as IContent;
            if (order != null) {
                _contentManager.Create(order);

                _shoppingCartService.Empty();

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
                if (cart.Properties["ShippingOption"] as ShippingProviderOption == null) {
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