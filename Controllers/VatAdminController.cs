using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Contents.ViewModels;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.Settings;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using OShop.Models;
using OShop.Permissions;
using OShop.Services;
using OShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [Admin]
    [OrchardFeature("OShop.VAT")]
    public class VatAdminController : Controller
    {
        private readonly IContentManager _contentManager;
        private readonly ISiteService _siteService;

        public VatAdminController(
            IContentManager contentManager,
            ISiteService siteService,
            IOrchardServices services,
            IShapeFactory shapeFactory
            ) {
            _contentManager = contentManager;
            _siteService = siteService;
            Services = services;

            T = NullLocalizer.Instance;

            Shape = shapeFactory;
        }

        dynamic Shape { get; set; }
        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }

        public ActionResult Index() {
            if (!Services.Authorizer.Authorize(OShopPermissions.ManageShopSettings, T("Not allowed to manage VAT Settings")))
                return new HttpUnauthorizedResult();

            var vatSettings = Services.WorkContext.CurrentSite.As<VatSettingsPart>();

            return View(vatSettings);
        }

        [HttpPost, ActionName("Index")]
        public ActionResult IndexPOST() {
            if (!Services.Authorizer.Authorize(OShopPermissions.ManageShopSettings, T("Not allowed to manage VAT Settings")))
                return new HttpUnauthorizedResult();

            var vatSettings = Services.WorkContext.CurrentSite.As<VatSettingsPart>();

            if (TryUpdateModel(vatSettings)) {
                Services.Notifier.Information(T("VAT Settings saved successfully."));
            }
            else {
                Services.Notifier.Error(T("Could not save VAT Settings."));
            }

            return Index();
        }

        public ActionResult Rates(ListContentsViewModel model, PagerParameters pagerParameters) {
            if (!Services.Authorizer.Authorize(OShopPermissions.ManageShopSettings, T("Not allowed to manage VAT rates")))
                return new HttpUnauthorizedResult();

            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);
            var query = _contentManager.Query<VatRatePart, VatRatePartRecord>(VersionOptions.Latest);

            switch (model.Options.OrderBy) {
                case ContentsOrder.Modified:
                    query.OrderByDescending<CommonPartRecord>(cr => cr.ModifiedUtc);
                    break;
                case ContentsOrder.Published:
                    query.OrderByDescending<CommonPartRecord>(cr => cr.PublishedUtc);
                    break;
                case ContentsOrder.Created:
                    query.OrderByDescending<CommonPartRecord>(cr => cr.CreatedUtc);
                    break;
            }

            var pagerShape = Shape.Pager(pager).TotalItemCount(query.Count());
            var pageOfContentItems = query.Slice(pager.GetStartIndex(), pager.PageSize).ToList();

            var list = Shape.List();
            list.AddRange(pageOfContentItems.Select(ci => _contentManager.BuildDisplay(ci, "SummaryAdmin")));

            var viewModel = Shape.ViewModel()
                .ContentItems(list)
                .Pager(pagerShape)
                .Options(model.Options);

            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View(viewModel);
        }

        [HttpPost, ActionName("Rates")]
        [FormValueRequired("submit.Filter")]
        public ActionResult RatesFilterPOST(ContentOptions options) {
            var routeValues = ControllerContext.RouteData.Values;
            if (options != null) {
                routeValues["Options.OrderBy"] = options.OrderBy; //todo: don't hard-code the key
            }

            return RedirectToAction("Rates", routeValues);
        }

        [HttpPost, ActionName("Rates")]
        [FormValueRequired("submit.BulkEdit")]
        public ActionResult RatesPOST(ContentOptions options, IEnumerable<int> itemIds, string returnUrl) {
            if (!Services.Authorizer.Authorize(OShopPermissions.ManageShopSettings, T("Not allowed to manage VAT rates")))
                return new HttpUnauthorizedResult();

            if (itemIds != null) {
                var checkedContentItems = _contentManager.GetMany<ContentItem>(itemIds, VersionOptions.Latest, QueryHints.Empty);
                switch (options.BulkAction) {
                    case ContentsBulkAction.None:
                        break;
                    case ContentsBulkAction.Remove:
                        foreach (var item in checkedContentItems) {
                            _contentManager.Remove(item);
                        }
                        Services.Notifier.Information(T("VAT rates successfully removed."));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Rates"));
        }

    }
}