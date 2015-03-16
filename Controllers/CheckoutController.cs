using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Environment.Features;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Security;
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
        private readonly IFeatureManager _featureManager;
        private readonly IShippingService _shippingService;

        public CheckoutController(
            ICustomersService customersService,
            IShoppingCartService shoppingCartService,
            ICurrencyProvider currencyProvider,
            IContentManager contentManager,
            IFeatureManager featureManager,
            IOrchardServices services,
            IShippingService shippingService = null
            ) {
            _customersService = customersService;
            _shoppingCartService = shoppingCartService;
            _currencyProvider = currencyProvider;
            _contentManager = contentManager;
            _featureManager = featureManager;
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

            // Billing address
            Int32 billingAddressId = _shoppingCartService.GetProperty<Int32>("BillingAddressId");
            if (billingAddressId <= 0) {
                if (customer.DefaultAddressId > 0 && customer.Addresses.Where(a => a.Id == customer.DefaultAddressId).Any()) {
                    billingAddressId = customer.DefaultAddressId;
                    _shoppingCartService.SetProperty<Int32>("BillingAddressId", billingAddressId);
                }
                else if(customer.Addresses.Any()) {
                    billingAddressId = customer.Addresses.First().ContentItem.Id;
                    _shoppingCartService.SetProperty<Int32>("BillingAddressId", billingAddressId);
                }
            }

            // Shipping address
            Int32 shippingAddressId = _shoppingCartService.GetProperty<Int32>("ShippingAddressId");
            if (shippingAddressId <= 0) {
                if (customer.DefaultAddressId > 0 && customer.Addresses.Where(a => a.Id == customer.DefaultAddressId).Any()) {
                    shippingAddressId = customer.DefaultAddressId;
                    _shoppingCartService.SetProperty<Int32>("ShippingAddressId", shippingAddressId);
                }
                else if (customer.Addresses.Any()) {
                    shippingAddressId = customer.Addresses.First().ContentItem.Id;
                    _shoppingCartService.SetProperty<Int32>("ShippingAddressId", shippingAddressId);
                }
            }

            ShoppingCart cart = _shoppingCartService.BuildCart();

            var model = new CheckoutIndexViewModel() {
                ShippingRequired = cart.IsShippingRequired(),
                Addresses = customer.Addresses,
                BillingAddressId = billingAddressId > 0 ? billingAddressId : customer.DefaultAddressId,
                ShippingAddressId = shippingAddressId > 0 ? shippingAddressId : customer.DefaultAddressId,
                NumberFormat = _currencyProvider.NumberFormat,
                VatEnabled = _featureManager.GetEnabledFeatures().Where(f => f.Id == "OShop.VAT").Any()
            };

            var billingAddress = _contentManager.Get(model.BillingAddressId, VersionOptions.Latest);
            if (billingAddress != null) {
                model.BillingAddress = _contentManager.BuildDisplay(billingAddress);
            }

            if (model.ShippingRequired && _shippingService != null) {
                var shippingAddress = _contentManager.Get(model.ShippingAddressId, VersionOptions.Latest);
                if (shippingAddress != null) {
                    model.ShippingAddress = _contentManager.BuildDisplay(shippingAddress);
                }

                model.ShippingProviders = _shippingService.GetSuitableProviderOptions(
                    cart.Properties["ShippingZone"] as ShippingZoneRecord,
                    cart.Properties["ShippingInfos"] as IList<Tuple<int, IShippingInfo>> ?? new List<Tuple<int, IShippingInfo>>(),
                    cart.ItemsTotal()
                ).OrderBy(p => p.Option.Price);
                var shippingOption = cart.Properties["ShippingOption"] as ShippingProviderOption;
                model.ShippingProviderId = shippingOption != null ? shippingOption.Provider.Id : 0;
            }

            return View(model);
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
            
            return View(_contentManager.BuildDisplay(order));
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
                //return RedirectToAction("Index", "Order", new { area = "OShop", ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
            }
        }

    }
}