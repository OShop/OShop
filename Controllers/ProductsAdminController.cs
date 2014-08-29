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
    [OrchardFeature("OShop.Products")]
    public class ProductsAdminController : Controller
    {
        // GET: ProductsAdmin
        public ActionResult List()
        {
            return View();
        }
    }
}