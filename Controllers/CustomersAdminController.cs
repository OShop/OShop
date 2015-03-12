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
    [OrchardFeature("OShop.Customers")]
    public class CustomersAdminController : Controller
    {
        private readonly IContentManager _contentManager;
        private readonly ISiteService _siteService;

        public CustomersAdminController(
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
            if (!Services.Authorizer.Authorize(Permissions.CustomersPermissions.ViewCustomerAccounts, T("Not allowed to manage customers")))
                return new HttpUnauthorizedResult();

            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);
            var query = _contentManager.Query<CustomerPart, CustomerPartRecord>(VersionOptions.Latest);

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
            if (!Services.Authorizer.Authorize(Permissions.CustomersPermissions.ManageCustomerAccounts, T("Not allowed to manage customers")))
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
            if (!Services.Authorizer.Authorize(Permissions.CustomersPermissions.ViewCustomerAccounts, T("Not allowed to manage customers")))
                return new HttpUnauthorizedResult();

            var customer = _contentManager.Get<CustomerPart>(id, VersionOptions.Latest);
            if (customer != null) {
                return new ShapeResult(this, _contentManager.BuildDisplay(customer.ContentItem, "DetailAdmin"));
            }
            else {
                return new HttpNotFoundResult();
            }
        }

        [HttpPost, ActionName("Detail")]
        [FormValueRequired("Action")]
        public ActionResult DetailPost(int id, string Action, int CustomerAddressId) {
            switch (Action) {
                case "Edit":
                    return RedirectToAction("Edit", "Admin", new { area = "Contents", id = CustomerAddressId, ReturnUrl = Url.Action("Detail", "CustomersAdmin", new { area = "OShop", id = id }) });
                case "Remove":
                    var contentItem = _contentManager.Get(CustomerAddressId, VersionOptions.Latest);

                    if (!Services.Authorizer.Authorize(Orchard.Core.Contents.Permissions.DeleteContent, contentItem, T("You are not allowed to remove this address.")))
                        return new HttpUnauthorizedResult();

                    if (contentItem != null) {
                        _contentManager.Remove(contentItem);
                        Services.Notifier.Information(T("Address was successfully removed."));
                    }
                    return Detail(id);
                default:
                    return Detail(id);
            }
        }

    }
}