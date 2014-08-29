using Orchard;
using Orchard.Localization;
using Orchard.UI.Admin;
using OShop.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [Admin]
    public class SettingsController : Controller
    {
        public SettingsController(IOrchardServices services) {
            Services = services;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        // GET: Settings
        public ActionResult Index()
        {
            if(!Services.Authorizer.Authorize(OShopPermissions.ManageShopSettings, T("Not allowed to manage Shop Settings")))
                return new HttpUnauthorizedResult();

            
            return View();
        }
    }
}