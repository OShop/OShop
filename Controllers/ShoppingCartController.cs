using Orchard.Environment.Extensions;
using Orchard.Environment.Features;
using Orchard.Themes;
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
    [OrchardFeature("OShop.ShoppingCart")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICurrencyProvider _currencyProvider;
        private readonly IFeatureManager _featureManager;

        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            ICurrencyProvider currencyProvider,
            IFeatureManager featureManager) {
            _shoppingCartService = shoppingCartService;
            _currencyProvider = currencyProvider;
            _featureManager = featureManager;
        }

        [Themed]
        public ActionResult Index()
        {
            var model = new ShoppingCartIndexViewModel() {
                CartItems = _shoppingCartService.ListItems(),
                NumberFormat = _currencyProvider.NumberFormat,
                // Optional features
                VatEnabled = _featureManager.GetEnabledFeatures().Where(f => f.Id == "OShop.VAT").Any()
            };

            return View(model);
        }

        public ActionResult Add(int id, int quantity = 1, string itemType = ProductPart.PartItemType, string returnUrl = null) {

            _shoppingCartService.Add(id, itemType, quantity);

            return ReturnOrIndex(returnUrl);
        }

        private RedirectResult ReturnOrIndex(string returnUrl = null) {
            var urlReferrer = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index");

            return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? urlReferrer : returnUrl);
        }
    }
}