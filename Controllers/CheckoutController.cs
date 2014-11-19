using Orchard.Environment.Extensions;
using Orchard.Mvc;
using Orchard.Security;
using Orchard.Themes;
using OShop.Helpers;
using OShop.Services;
using OShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [OrchardFeature("OShop.Checkout")]
    public class CheckoutController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomersService _customersService;
        private readonly IShoppingCartService _shoppingCartService;

        public CheckoutController(
            IAuthenticationService authenticationService,
            ICustomersService customersService,
            IShoppingCartService shoppingCartService
            ) {
            _authenticationService = authenticationService;
            _customersService = customersService;
            _shoppingCartService = shoppingCartService;
        }

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

            Int32 billingAddressId = _shoppingCartService.GetProperty<Int32>("BillingAddressId");
            Int32 shippingAddressId = _shoppingCartService.GetProperty<Int32>("ShippingAddressId");
            var model = new CheckoutIndexViewModel() {
                ShippingRequired = _shoppingCartService.GetShoppingCart().IsShippingRequired(),
                Addresses = _customersService.GetAddresses(user.Id),
                BillingAddressId = billingAddressId > 0 ? billingAddressId : customer.DefaultAddressId,
                ShippingAddressId = shippingAddressId > 0 ? shippingAddressId : customer.DefaultAddressId
            };

            return View(model);
        }

        [HttpPost, ActionName("Index")]
        [FormValueRequired("Action")]
        public ActionResult IndexPost(string Action, CheckoutIndexViewModel Model) {
            var user = _authenticationService.GetAuthenticatedUser();
            if (user == null) {
                return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
            }

            switch (Action) {
                case "EditShippingAddress":
                    _shoppingCartService.SetProperty("ShippingAddressId", Model.ShippingAddressId);
                    return RedirectToAction("EditAddress", "Customer", new { area = "OShop", id = Model.ShippingAddressId, ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
                case "RemoveShippingAddress":
                    return RedirectToAction("RemoveAddress", "Customer", new { area = "OShop", id = Model.ShippingAddressId, ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
                case "EditBillingAddress":
                    _shoppingCartService.SetProperty("BillingAddressId", Model.BillingAddressId);
                    return RedirectToAction("EditAddress", "Customer", new { area = "OShop", id = Model.BillingAddressId, ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
                case "RemoveBillingAddress":
                    return RedirectToAction("RemoveAddress", "Customer", new { area = "OShop", id = Model.BillingAddressId, ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
                default:
                    return Index();
            }
        }

    }
}