using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Mvc;
using Orchard.Settings;
using Orchard.Themes;
using Orchard.UI.Navigation;
using OShop.Services;
using OShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [OrchardFeature("OShop.Orders")]
    public class OrdersController : Controller
    {
        private readonly IOrdersService _ordersService;
        private readonly IContentManager _contentManager;
        private readonly ISiteService _siteService;

        public OrdersController(
            IContentManager contentManager,
            IOrdersService ordersService,
            ISiteService siteService,
            IShapeFactory shapeFactory
            ) {
            _contentManager = contentManager;
            _ordersService = ordersService;
            _siteService = siteService;

            Shape = shapeFactory;
        }

        dynamic Shape { get; set; }

        // GET: Order
        [Themed]
        [Authorize]
        public ActionResult Index(PagerParameters pagerParameters)
        {
            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

            var myOrders = _ordersService.GetMyOrders();

            var model = new OrdersIndexViewModel() {
                Orders = myOrders.Skip(pager.GetStartIndex()).Take(pager.PageSize)
                    .Select(order => _contentManager.BuildDisplay(order.ContentItem, "Summary")),
                Pager = Shape.Pager(pager).TotalItemCount(myOrders.Count())
            };

            return View(model);
        }

        [Themed]
        public ActionResult Display(string id) {
            var order = _ordersService.GetOrderByReference(id);

            if (order == null) {
                return new HttpNotFoundResult();
            }
            else {
                return new ShapeResult(this, _contentManager.BuildDisplay(order.ContentItem));
            }
        }
    }
}