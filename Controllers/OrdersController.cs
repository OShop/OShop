using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Mvc;
using Orchard.Themes;
using OShop.Services;
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

        public OrdersController(
            IContentManager contentManager, 
            IOrdersService ordersService
            ) {
            _contentManager = contentManager;
            _ordersService = ordersService;
        }

        // GET: Order
        [Themed]
        public ActionResult Index()
        {
            return View();
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