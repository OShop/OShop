using Orchard.Environment.Extensions;
using Orchard.Security;
using Orchard.Themes;
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

        public CheckoutController(
            IAuthenticationService authenticationService
            ) {
            _authenticationService = authenticationService;
        }

        [Themed]
        public ActionResult Index()
        {
            if (_authenticationService.GetAuthenticatedUser() != null) {
                return View();
            }
            else {
                return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = Url.Action("Index", "Checkout", new { area = "OShop" }) });
            }
        }
    }
}