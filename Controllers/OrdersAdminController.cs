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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [Admin]
    [OrchardFeature("OShop.Orders")]
    public class OrdersAdminController : Controller
    {
        private readonly IContentManager _contentManager;
        private readonly ISiteService _siteService;

        public OrdersAdminController(
            IOrchardServices services,
            IContentManager contentManager,
            ISiteService siteService,
            IShapeFactory shapeFactory) {
            Services = services;
            _contentManager = contentManager;
            _siteService = siteService;
            T = NullLocalizer.Instance;
            Shape = shapeFactory;
        }

        dynamic Shape { get; set; }
        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }

        public ActionResult Index(ListContentsViewModel model, PagerParameters pagerParameters) {
            if (!Services.Authorizer.Authorize(Permissions.OrdersPermissions.ViewOrders, T("Not allowed to view orders")))
                return new HttpUnauthorizedResult();

            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);
            var query = _contentManager.Query<OrderPart, OrderPartRecord>(VersionOptions.Latest);

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

        [HttpPost, ActionName("Index")]
        [FormValueRequired("submit.Filter")]
        public ActionResult IndexFilterPOST(ContentOptions options) {
            var routeValues = ControllerContext.RouteData.Values;
            if (options != null) {
                routeValues["Options.OrderBy"] = options.OrderBy; //todo: don't hard-code the key
            }

            return RedirectToAction("Index", routeValues);
        }

        [HttpPost, ActionName("Index")]
        [FormValueRequired("submit.BulkEdit")]
        public ActionResult IndexPOST(ContentOptions options, IEnumerable<int> itemIds, string returnUrl) {
            if (!Services.Authorizer.Authorize(Permissions.OrdersPermissions.ManageOrders, T("Not allowed to manage orders")))
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
                        Services.Notifier.Information(T("Customers successfully removed."));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        public ActionResult Detail(int id) {
            if (!Services.Authorizer.Authorize(Permissions.OrdersPermissions.ViewOrders, T("Not allowed to view orders")))
                return new HttpUnauthorizedResult();

            var order = _contentManager.Get<OrderPart>(id, VersionOptions.Latest);
            if (order != null) {
                return new ShapeResult(this, _contentManager.BuildDisplay(order.ContentItem, "Detail"));
            }
            else {
                return new HttpNotFoundResult();
            }
        }

        [HttpPost, ActionName("Detail")]
        [FormValueRequired("Action")]
        public ActionResult DetailPost(int id, string Action) {
            switch (Action) {
                default:
                    return Detail(id);
            }
        }

    }
}