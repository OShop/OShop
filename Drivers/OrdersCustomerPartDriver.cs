using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using OShop.Models;
using OShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Customers")]
    public class OrdersCustomerPartDriver : ContentPartDriver<CustomerPart> {
        private readonly IContentManager _contentManager;
        private readonly ICustomersService _customersService;

        public OrdersCustomerPartDriver(
            IContentManager contentManager,
            ICustomersService customersService,
            IOrchardServices services
            ) {
            _contentManager = contentManager;
            _customersService = customersService;
            Services = services;
        }

        public IOrchardServices Services { get; set; }

        private const string pageKey = "customer-orders-page";

        protected override DriverResult Display(CustomerPart part, string displayType, dynamic shapeHelper) {
            bool isAdmin = AdminFilter.IsApplied(Services.WorkContext.HttpContext.Request.RequestContext);

            if (isAdmin) {
                if (!Services.Authorizer.Authorize(Permissions.OrdersPermissions.ViewOrders)) {
                    return null;
                }
            }
            else {
                var customer = _customersService.GetCustomer();
                if (customer == null || customer.ContentItem.Id != part.ContentItem.Id || !Services.Authorizer.Authorize(Permissions.OrdersPermissions.ViewOwnOrders)) {
                    return null;
                }
            }

            if (part.ContentItem.Id > 0) {
                Int32 page = 0;
                if(Services.WorkContext.HttpContext.Request.QueryString.AllKeys.Contains(pageKey)){
                    Int32.TryParse(Services.WorkContext.HttpContext.Request.QueryString[pageKey], out page);
                }
                var pager = new Pager(Services.WorkContext.CurrentSite, page, null);

                var orders = _contentManager.Query<CustomerOrderPart, CustomerOrderPartRecord>()
                    .Where(o => o.CustomerId == part.ContentItem.Id)
                    .Join<CommonPartRecord>()
                    .OrderByDescending(c => c.CreatedUtc);

                var pagerShape = shapeHelper.Pager(pager)
                    .PagerId(pageKey)
                    .TotalItemCount(orders.Count());

                var list = shapeHelper.List();

                if (isAdmin) {
                    list.AddRange(orders.Slice(pager.GetStartIndex(), pager.PageSize)
                        .Select(o => _contentManager.BuildDisplay(o, "SummaryAdmin")));
                    return ContentShape("Parts_Customer_Orders_Admin", () => shapeHelper.Parts_Customer_Orders_Admin(
                        ContentItems: list,
                        Pager: pagerShape
                    ));
                }
                else {
                    list.AddRange(orders.Slice(pager.GetStartIndex(), pager.PageSize)
                        .Select(o => _contentManager.BuildDisplay(o, "Summary")));
                    return ContentShape("Parts_Customer_Orders", () => shapeHelper.Parts_Customer_Orders(
                        ContentItems: list,
                        Pager: pagerShape
                    ));
                }
            }
            else {
                return null;
            }
        }
    }
}