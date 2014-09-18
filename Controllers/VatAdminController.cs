using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using OShop.Models;
using OShop.Services;
using OShop.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [Admin]
    [OrchardFeature("OShop.VAT")]
    public class VatAdminController : Controller
    {
        private readonly IVatService _vatService;

        public VatAdminController(
            IVatService vatService,
            IOrchardServices services
            ) {
            _vatService = vatService;
            Services = services;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }

        public ActionResult List() {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage VAT")))
                return new HttpUnauthorizedResult();

            var records = _vatService.ListVats().OrderBy(v => v.Name);

            if (!records.Any()) {
                Services.Notifier.Information(T("There is no VAT created for now."));
            }

            return View(records);
        }

        public ActionResult Create() {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to create VAT")))
                return new HttpUnauthorizedResult();

            return View();
        }

        [HttpPost]
        public ActionResult Create(VatCreateViewModel model) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to create VAT")))
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid) {
                _vatService.AddVat(new VatRecord() {
                    Name = model.Name,
                    Rate = model.Rate
                });
                Services.Notifier.Information(T("VAT {0} successfully created.", model.Name));
                return RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult Edit(int id) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to create VAT")))
                return new HttpUnauthorizedResult();

            VatRecord model = _vatService.GetVat(id);

            if (model == null) {
                return new HttpNotFoundResult();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, VatRecord model) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to create VAT")))
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid) {
                _vatService.UpdateVat(model);
                Services.Notifier.Information(T("VAT {0} successfully updated.", model.Name));
            }

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to create VAT")))
                return new HttpUnauthorizedResult();
            VatRecord record = _vatService.GetVat(id);
            if (record != null) {
                _vatService.DeleteVat(id);
                Services.Notifier.Information(T("VAT {0} successfully deleted.", record.Name));
            }
            else {
                Services.Notifier.Warning(T("Unable to find VAT with id {0}", id));
            }
            return RedirectToAction("List");
        }

    }
}