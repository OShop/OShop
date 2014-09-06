using Orchard.Environment.Extensions;
using Orchard.Themes;
using OShop.Services;
using OShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [OrchardFeature("OShop.ShoppingCart")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICurrencyProvider _currencyProvider;

        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            ICurrencyProvider currencyProvider) {
            _shoppingCartService = shoppingCartService;
            _currencyProvider = currencyProvider;
        }

        [Themed]
        public ActionResult Index()
        {
            var model = new ShoppingCartIndexViewModel() {
                CartItems = _shoppingCartService.ListItems(),
                NumberFormat = _currencyProvider.NumberFormat
            };

            return View(model);
        }

        public ActionResult Add(int id, int quantity = 1, string itemType = "Product", string returnUrl = null) {

            _shoppingCartService.Add(id, itemType, quantity);

            return ReturnOrIndex(returnUrl);
        }

        private RedirectResult ReturnOrIndex(string returnUrl = null) {
            var urlReferrer = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index");

            return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? urlReferrer : returnUrl);
        }
    }
}