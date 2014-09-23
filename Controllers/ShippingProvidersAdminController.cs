using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Common.Models;
using Orchard.Core.Contents.ViewModels;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc.Extensions;
using Orchard.Mvc.Html;
using Orchard.Settings;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using OShop.Models;
using OShop.Services;
using OShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [Admin]
    [OrchardFeature("OShop.Shipping")]
    public class ShippingProvidersAdminController : Controller {
        private readonly IContentManager _contentManager;
        private readonly ISiteService _siteService;
        private readonly IShippingService _shippingService;

        public ShippingProvidersAdminController(
            IOrchardServices services,
            IContentManager contentManager,
            ISiteService siteService,
            IShippingService shippingService,
            IShapeFactory shapeFactory) {
            Services = services;
            _contentManager = contentManager;
            _siteService = siteService;
            _shippingService = shippingService;
            T = NullLocalizer.Instance;
            Shape = shapeFactory;
        }

        dynamic Shape { get; set; }
        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }

        public ActionResult Index(ListContentsViewModel model, PagerParameters pagerParameters)
        {
            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);
            var query = _contentManager.Query<ShippingProviderPart, ShippingProviderPartRecord>(VersionOptions.Latest);

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

            dynamic viewModel = Shape.ViewModel()
                .ContentItems(list)
                .Pager(pagerShape)
                .Options(model.Options);

            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)viewModel);
        }

        public ActionResult AddOption(int id) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage shipping options")))
                return new HttpUnauthorizedResult();

            var provider = _contentManager.Get<ShippingProviderPart>(id);

            var Model = new ShippingOptionAddViewModel() {
                ShippingProviderId = id,
                ShippingProviderName = provider.As<ITitleAspect>().Title,
                ShippingZones = _shippingService.GetZones(),

                ReturnUrl = Url.ItemAdminUrl(provider)
            };

            return View(Model);
        }

        [HttpPost]
        public ActionResult AddOption(ShippingOptionAddViewModel model) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage shipping options")))
                return new HttpUnauthorizedResult();

            var provider = _contentManager.Get<ShippingProviderPart>(model.ShippingProviderId);

            if (ModelState.IsValid) {
                _shippingService.CreateOption(new ShippingOptionRecord() {
                    Name = model.Name,
                    Enabled = model.Enabled,
                    ShippingZoneRecord = (model.ShippingZoneId > 0 ? _shippingService.GetZone(model.ShippingZoneId) : null),
                    ShippingProviderPartRecord = provider.Record,
                    Priority = model.Priority,

                    Price = model.Price
                });
                Services.Notifier.Information(T("Shipping option {0} successfully created.", model.Name));
                return Redirect(Url.ItemAdminUrl(provider));
            }
            else {
                model.ShippingProviderName = _contentManager.Get<ShippingProviderPart>(model.ShippingProviderId).As<ITitleAspect>().Title;
                model.ReturnUrl = Url.ItemAdminUrl(provider);
                model.ShippingZones = _shippingService.GetZones();
                return View(model);
            }
        }

        public ActionResult EditOption(int id) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage shipping options")))
                return new HttpUnauthorizedResult();

            var option = _shippingService.GetOption(id);
            var provider = _contentManager.Get<ShippingProviderPart>(option.ShippingProviderPartRecord.Id);

            var model = new ShippingOptionEditViewModel() {
                OptionId = option.Id,
                ShippingProviderId = provider.Id,
                ShippingProviderName = provider.As<ITitleAspect>().Title,
                Name = option.Name,
                Enabled = option.Enabled,
                ShippingZones = _shippingService.GetZones(),
                ShippingZoneId = (option.ShippingZoneRecord != null ? option.ShippingZoneRecord.Id : 0),
                Priority = option.Priority,
                Price = option.Price,
                ReturnUrl = Url.ItemAdminUrl(provider)
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditOption(ShippingOptionEditViewModel model) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage shipping options")))
                return new HttpUnauthorizedResult();

            var provider = _contentManager.Get<ShippingProviderPart>(model.ShippingProviderId);

            if (ModelState.IsValid) {
                var option = _shippingService.GetOption(model.OptionId);

                option.Name = model.Name;
                option.Enabled = model.Enabled;
                option.ShippingZoneRecord = (model.ShippingZoneId > 0 ? _shippingService.GetZone(model.ShippingZoneId) : null);
                option.Priority = model.Priority;
                option.Price = model.Price;

                _shippingService.UpdateOption(option);
                
                Services.Notifier.Information(T("Shipping option {0} successfully updated.", model.Name));

                return Redirect(Url.ItemAdminUrl(provider));
            }
            else {
                model.ShippingProviderName = _contentManager.Get<ShippingProviderPart>(model.ShippingProviderId).As<ITitleAspect>().Title;
                model.ReturnUrl = Url.ItemAdminUrl(provider);
                model.ShippingZones = _shippingService.GetZones();
                return View(model);
            }
        }

        public ActionResult DeleteOption(int id) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage shipping options")))
                return new HttpUnauthorizedResult();

            var record = _shippingService.GetOption(id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            var shippingProvider = _contentManager.Get<ShippingProviderPart>(record.ShippingProviderPartRecord.Id);

            _shippingService.DeleteOption(record);

            Services.Notifier.Information(T("Option {0} successfully deleted.", record.Name));

            return Redirect(Url.ItemAdminUrl(shippingProvider));
        }

        public ActionResult EnableOption(int id) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage shipping options")))
                return new HttpUnauthorizedResult();

            var record = _shippingService.GetOption(id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            var shippingProvider = _contentManager.Get<ShippingProviderPart>(record.ShippingProviderPartRecord.Id);

            record.Enabled = true;
            _shippingService.UpdateOption(record);

            Services.Notifier.Information(T("Option {0} successfully enabled.", record.Name));

            return Redirect(Url.ItemAdminUrl(shippingProvider));
        }

        public ActionResult DisableOption(int id) {
            if (!Services.Authorizer.Authorize(Permissions.OShopPermissions.ManageShopSettings, T("Not allowed to manage shipping options")))
                return new HttpUnauthorizedResult();

            var record = _shippingService.GetOption(id);

            if (record == null) {
                return new HttpNotFoundResult();
            }

            var shippingProvider = _contentManager.Get<ShippingProviderPart>(record.ShippingProviderPartRecord.Id);

            record.Enabled = false;
            _shippingService.UpdateOption(record);

            Services.Notifier.Information(T("Option {0} successfully disabled.", record.Name));

            return Redirect(Url.ItemAdminUrl(shippingProvider));
        }

    }
}