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
using System.Linq;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [OrchardFeature("OShop.Checkout")]
    public class CheckoutController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomersService _customersService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICurrencyProvider _currencyProvider;
        private readonly IContentManager _contentManager;
        private readonly IFeatureManager _featureManager;
        private readonly IShippingService _shippingService;

        public CheckoutController(
            IAuthenticationService authenticationService,
            ICustomersService customersService,
            IShoppingCartService shoppingCartService,
            ICurrencyProvider currencyProvider,
            IContentManager contentManager,
            IFeatureManager featureManager,
            IOrchardServices services,
            IShippingService shippingService = null
            ) {
            _authenticationService = authenticationService;
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
        public ActionResult Index()
        {
            var user = _authenticationService.GetAuthenticatedUser();
            if (user == null) {
                return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
            }

            var customer = _customersService.GetCustomer(user.Id);
            if(customer == null) {
                return RedirectToAction("Create", "Customer", new { area = "OShop", ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
            }

            ShoppingCart cart = _shoppingCartService.BuildCart();

            Int32 billingAddressId = _shoppingCartService.GetProperty<Int32>("BillingAddressId");
            Int32 shippingAddressId = _shoppingCartService.GetProperty<Int32>("ShippingAddressId");
            var model = new CheckoutIndexViewModel() {
                ShippingRequired = cart.IsShippingRequired(),
                Addresses = _customersService.GetAddresses(user.Id),
                BillingAddressId = billingAddressId > 0 ? billingAddressId : customer.DefaultAddressId,
                ShippingAddressId = shippingAddressId > 0 ? shippingAddressId : customer.DefaultAddressId,
                NumberFormat = _currencyProvider.NumberFormat,
                VatEnabled = _featureManager.GetEnabledFeatures().Where(f => f.Id == "OShop.VAT").Any()
            };

            model.BillingAddress = _contentManager.BuildDisplay(_contentManager.Get(model.BillingAddressId));
            model.ShippingAddress = _contentManager.BuildDisplay(_contentManager.Get(model.ShippingAddressId));

            if (model.ShippingRequired && _shippingService != null) {
                model.ShippingProviders = _shippingService.GetSuitableProviderOptions(cart).OrderBy(p => p.Option.Price);
                model.ShippingProviderId = cart.ShippingOption != null ? cart.ShippingOption.Provider.Id : 0;
            }

            return View(model);
        }

        [Themed]
        [HttpPost, ActionName("Index")]
        public ActionResult IndexPost(string Action, CheckoutIndexViewModel Model) {
            var user = _authenticationService.GetAuthenticatedUser();
            if (user == null) {
                return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
            }

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
        public ActionResult ValidateOrder() {
            var user = _authenticationService.GetAuthenticatedUser();
            if (user == null) {
                return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
            }

            var order = _shoppingCartService.BuildOrder();
            
            return View(_contentManager.BuildDisplay(order));
        }

        private ActionResult ValidateAddress() {
            var cart = _shoppingCartService.BuildCart();
            Boolean isValid = cart.IsValid;

            if (cart.BillingAddress == null) {
                isValid = false;
                Services.Notifier.Error(T("Please provide your billing address."));
            }

            if (cart.IsShippingRequired()) {
                if (cart.ShippingAddress == null) {
                    isValid = false;
                    Services.Notifier.Error(T("Please provide your shipping address."));
                }
                if (cart.ShippingOption == null) {
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