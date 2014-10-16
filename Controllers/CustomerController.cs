using Orchard.Environment.Extensions;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [OrchardFeature("OShop.Customers")]
    public class CustomerController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public CustomerController(
            IAuthenticationService authenticationService
            ) {
            _authenticationService = authenticationService;
        }

        public ActionResult Index()
        {
            var user = _authenticationService.GetAuthenticatedUser();
            if (user == null) {
                return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = Url.Action("Index", "Customer", new { area = "OShop" }) });
            }
            
            return View();
        }

        public ActionResult Create() {
            return View();
        }
    }
}