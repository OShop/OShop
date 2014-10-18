using Orchard.Environment.Extensions;
using Orchard.Security;
using Orchard.Themes;
using OShop.Services;
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

        public CheckoutController(
            IAuthenticationService authenticationService,
            ICustomersService customersService
            ) {
            _authenticationService = authenticationService;
            _customersService = customersService;
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

            return View();
        }
    }
}