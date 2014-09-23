using Orchard;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.Mvc.Html;
using Orchard.Settings;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using OShop.Models;
using OShop.Services;
using OShop.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [Admin]
    [OrchardFeature("OShop.Shipping")]
    public class ShippingZonesAdminController : Controller
    {
        private readonly IShippingService _shippingService;
        private readonly ISiteService _siteService;

        public ShippingZonesAdminController(
            IShippingService shippingService,
            ISiteService siteService,
            IOrchardServices services,
            IShapeFactory shapeFactory) {
            _shippingService = shippingService;
            _siteService = siteService;

            Shape = shapeFactory;

            Services = services;

            T = NullLocalizer.Instance;
        }

        dynamic Shape { get; set; }
        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }

        public ActionResult Index(ShippingZonesIndexViewModel model, PagerParameters pagerParameters) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage shipping zones")))
                return new HttpUnauthorizedResult();

            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

            var zones = _shippingService.GetZones();

            switch (model.Filter) {
                case ShippingZoneFilter.Disabled:
                    zones = zones.Where(z => !z.Enabled);
                    break;
                case ShippingZoneFilter.Enabled:
                    zones = zones.Where(z => z.Enabled);
                    break;
            }

            var pagerShape = Shape.Pager(pager).TotalItemCount(zones.Count());

            var viewModel = new ShippingZonesIndexViewModel() {
                Zones = zones.Skip(pager.GetStartIndex()).Take(pager.PageSize),
                Pager = pagerShape,
                BulkAction = ShippingZoneBulkAction.None,
                Filter = model.Filter
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Index")]
        [FormValueRequired("submit.BulkEdit")]
        public ActionResult IndexPOST(ShippingZonesIndexViewModel model, IEnumerable<int> itemIds, string returnUrl) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage shipping zones")))
                return new HttpUnauthorizedResult();

            if (itemIds != null && model.BulkAction != ShippingZoneBulkAction.None) {
                int counter = 0;
                foreach (int itemId in itemIds) {
                    var zone = _shippingService.GetZone(itemId);
                    if (zone != null) {
                        switch (model.BulkAction) {
                            case ShippingZoneBulkAction.Enable:
                                zone.Enabled = true;
                                break;
                            case ShippingZoneBulkAction.Disable:
                                zone.Enabled = false;
                                break;
                            case ShippingZoneBulkAction.Remove:
                                _shippingService.DeleteZone(zone);
                                break;
                        }

                        counter++;
                    }
                }

                switch (model.BulkAction) {
                    case ShippingZoneBulkAction.Enable:
                        Services.Notifier.Information(T.Plural("One zone successfully enabled.", "{0} zones successfully enabled.", counter));
                        break;
                    case ShippingZoneBulkAction.Disable:
                        Services.Notifier.Information(T.Plural("One zone successfully disabled.", "{0} zones successfully disabled.", counter));
                        break;
                    case ShippingZoneBulkAction.Remove:
                        Services.Notifier.Information(T.Plural("One zone successfully deleted.", "{0} zones successfully deleted.", counter));
                        break;
                }
            }

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        public ActionResult Add() {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage shipping zones")))
                return new HttpUnauthorizedResult();

            return View();
        }

        [HttpPost]
        public ActionResult Add(ShippingZonesAddViewModel model) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage shipping zones")))
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid) {
                _shippingService.CreateZone(new ShippingZoneRecord() {
                    Name = model.Name,
                    Enabled = model.Enabled
                });
                Services.Notifier.Information(T("Shipping zone {0} successfully created.", model.Name));
                return RedirectToAction("Index");
            }

            return View(model);

        }

        public ActionResult Edit(int id) {
            var record = _shippingService.GetZone(id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            return View(record);
        }

        [HttpPost]
        public ActionResult Edit(int id, ShippingZoneRecord model) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage shipping zones")))
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid) {
                _shippingService.UpdateZone(model);

                Services.Notifier.Information(T("Country {0} successfully updated.", model.Name));
            }

            return View(model);
        }

        public ActionResult Enable(int id, string returnUrl = null) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage shipping zones")))
                return new HttpUnauthorizedResult();

            var record = _shippingService.GetZone(id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            record.Enabled = true;

            Services.Notifier.Information(T("Zone {0} successfully enabled.", record.Name));

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        public ActionResult Disable(int id, string returnUrl = null) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage shipping zones")))
                return new HttpUnauthorizedResult();

            var record = _shippingService.GetZone(id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            record.Enabled = false;

            Services.Notifier.Information(T("Zone {0} successfully disabled.", record.Name));

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        public ActionResult Delete(int id, string returnUrl = null) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage shipping zones")))
                return new HttpUnauthorizedResult();

            var record = _shippingService.GetZone(id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            _shippingService.DeleteZone(record);

            Services.Notifier.Information(T("Zone {0} successfully deleted.", record.Name));

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

    }
}