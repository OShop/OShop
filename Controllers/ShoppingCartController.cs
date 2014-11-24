using Orchard.Environment.Extensions;
using Orchard.Environment.Features;
using Orchard.Themes;
using OShop.Models;
using OShop.Services;
using OShop.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [OrchardFeature("OShop.ShoppingCart")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICurrencyProvider _currencyProvider;
        private readonly IFeatureManager _featureManager;

        private readonly ILocationsService _locationService;
        private readonly IShippingService _shippingService;

        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            ICurrencyProvider currencyProvider,
            IFeatureManager featureManager,
            ILocationsService locationService = null,
            IShippingService shippingService = null) {
            _shoppingCartService = shoppingCartService;
            _currencyProvider = currencyProvider;
            _featureManager = featureManager;
            _locationService = locationService;
            _shippingService = shippingService;
        }

        [Themed]
        [OutputCache(Duration = 0)]
        public ActionResult Index()
        {

            ShoppingCart cart = _shoppingCartService.GetShoppingCart();

            var model = new ShoppingCartIndexViewModel() {
                Cart = cart,
                NumberFormat = _currencyProvider.NumberFormat,
                // Optional features
                VatEnabled = _featureManager.GetEnabledFeatures().Where(f => f.Id == "OShop.VAT").Any(),
                CheckoutEnabled = _featureManager.GetEnabledFeatures().Where(f => f.Id == "OShop.Checkout").Any(),
                ExpressCheckoutEnabled = _featureManager.GetEnabledFeatures().Where(f => f.Id == "OShop.ExpressCheckout").Any()
            };

            if (_locationService != null) {
                model.Countries = _locationService.GetEnabledCountries();
                model.States = _locationService.GetEnabledStates(cart.ShippingAddress != null ? cart.ShippingAddress.CountryId : 0);
            }

            if (_shippingService != null) {
                model.ShippingProviders = _shippingService.GetSuitableProviderOptions(cart).OrderBy(p => p.Option.Price);
            }

            return View(model);
        }

        [Themed]
        [HttpPost, ActionName("Index")]
        public ActionResult IndexPost(ShoppingCartUpdateViewModel model) {
            if (model.CartItems != null) {
                UpdateCart(model.CartItems);
            }

            if (_locationService != null) {
                var country = _locationService.GetCountry(model.CountryId);

                if (country != null && country.Enabled) {
                    _shoppingCartService.SetProperty("CountryId", country.Id);
                    var state = country.States.Where(s => s.Id == model.StateId && s.Enabled).FirstOrDefault();
                    if (state != null) {
                        _shoppingCartService.SetProperty("StateId", state.Id);
                    }
                    else {
                        _shoppingCartService.RemoveProperty("StateId");
                    }
                }
                else {
                    _shoppingCartService.RemoveProperty("CountryId");
                    _shoppingCartService.RemoveProperty("StateId");
                }

            }

            if (_shippingService != null) {
                if (model.ShippingProviderId > 0) {
                    _shoppingCartService.SetProperty("ShippingProviderId", model.ShippingProviderId);
                }
                else {
                    _shoppingCartService.RemoveProperty("ShippingProviderId");
                }
            }

            if (model.Action == "Checkout") {
                return RedirectToAction("Index", "Checkout", new { area = "OShop" });
            }
            else {
                return Index();
            }
        }

        public ActionResult Add(int id, int quantity = 1, string itemType = ProductPart.PartItemType, string returnUrl = null) {

            _shoppingCartService.Add(id, itemType, quantity);

            return ReturnOrIndex(returnUrl);
        }

        public ActionResult Remove(int id) {
            _shoppingCartService.Remove(id);

            return RedirectToAction("Index");
        }

        public ActionResult Empty(string returnUrl = null) {
            _shoppingCartService.Empty();

            return ReturnOrIndex(returnUrl);
        }

        [OutputCache(Duration = 0)]
        public ActionResult Widget() {
            ShoppingCart cart = _shoppingCartService.GetShoppingCart();
            var model = new ShoppingCartWidgetViewModel() {
                Cart = cart,
                NumberFormat = _currencyProvider.NumberFormat,
                // Optional features
                VatEnabled = _featureManager.GetEnabledFeatures().Where(f => f.Id == "OShop.VAT").Any()
            };

            return View(model);
        }

        private RedirectResult ReturnOrIndex(string returnUrl = null) {
            var urlReferrer = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index");

            return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? urlReferrer : returnUrl);
        }

        private void UpdateCart(ShoppingCartItemUpdateViewModel[] CartItems) {
            foreach (var item in CartItems) {
                if (item.IsRemoved) {
                    _shoppingCartService.Remove(item.Id);
                }
                else {
                    _shoppingCartService.UpdateQuantity(item.Id, item.Quantity);
                }
            }
        }

    }
}