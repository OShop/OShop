using Orchard.Environment.Extensions;
using Orchard.UI.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [Admin]
    [OrchardFeature("OShop.Shipping")]
    public class ShippingProvidersAdminController : Controller
    {
        // GET: ShippingProvidersAdmin
        public ActionResult Index()
        {
            return View();
        }
    }
}