using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Mvc;
using Orchard.Themes;
using OShop.Helpers;
using OShop.Models;
using OShop.Services;
using OShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [OrchardFeature("OShop.ShoppingCart")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICurrencyProvider _currencyProvider;
        private readonly IEnumerable<ICheckoutProvider> _checkoutProviders;
        private readonly ILocationsService _locationService;
        private readonly IShippingService _shippingService;
        private readonly dynamic _shapeFactory;


        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            ICurrencyProvider currencyProvider,
            IEnumerable<ICheckoutProvider> checkoutProviders,
            IShapeFactory shapeFactory,
            ILocationsService locationService = null,
            IShippingService shippingService = null) {
            _shoppingCartService = shoppingCartService;
            _currencyProvider = currencyProvider;
            _checkoutProviders = checkoutProviders;
            _shapeFactory = shapeFactory;
            _locationService = locationService;
            _shippingService = shippingService;
        }

        [Themed]
        [OutputCache(Duration = 0)]
        public ActionResult Index()
        {
            ShoppingCart cart = _shoppingCartService.BuildCart();

            if (!cart.Items.Any()) {
                return new ShapeResult(this, _shapeFactory.ShoppingCart_Empty());
            }

            var shoppingCartShape = _shapeFactory.ShoppingCart();

            shoppingCartShape.CartItems = _shapeFactory.ShoppingCart_CartItems()
                .Cart(cart)
                .NumberFormat(_currencyProvider.NumberFormat);

            if (_locationService != null && cart.Properties["BillingAddress"] as IOrderAddress == null) {
                // No address selected => show location selection
                int countryId = _shoppingCartService.GetProperty<int>("CountryId");

                shoppingCartShape.LocationEdit = _shapeFactory.ShoppingCart_LocationEdit()
                    .CountryId(countryId)
                    .StateId(_shoppingCartService.GetProperty<int>("StateId"))
                    .Countries(_locationService.GetEnabledCountries())
                    .States(_locationService.GetEnabledStates(countryId));
            }

            if (_shippingService != null && cart.IsShippingRequired()) {
                // Shipping option selection
                shoppingCartShape.ShippingOptions = _shapeFactory.ShoppingCart_ShippingOptions()
                    .Cart(cart)
                    .ContentItems(_shapeFactory.List()
                        .AddRange(_shippingService.GetSuitableProviderOptions(
                                cart.Properties["ShippingZone"] as ShippingZoneRecord,
                                cart.Properties["ShippingInfos"] as IList<Tuple<int, IShippingInfo>> ?? new List<Tuple<int, IShippingInfo>>(),
                                cart.ItemsTotal()
                            ).OrderBy(p => p.Option.Price)
                            .Select(sp => _shapeFactory.ShoppingCart_ShippingOption()
                                .Cart(cart)
                                .ProviderOption(sp)
                                .NumberFormat(_currencyProvider.NumberFormat)
                            )
                        )
                    );

            }

            // Actions
            shoppingCartShape.Actions = _shapeFactory.ShoppingCart_Actions()
                .Cart(cart)
                .CheckoutProviders(_checkoutProviders.OrderByDescending(cp => cp.Priority));

            return new ShapeResult(this, shoppingCartShape);
        }

        [Themed]
        [HttpPost, ActionName("Index")]
        public ActionResult IndexPost(ShoppingCartIndexViewModel model, string Checkout) {
            if (model.CartItems != null) {
                UpdateCart(model.CartItems);
            }

            if (_locationService != null && model.CountryId > 0) {
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

            if (!String.IsNullOrEmpty(Checkout)) {
                var checkoutProvider = _checkoutProviders.Where(cp => cp.Name == Checkout).OrderByDescending(cp => cp.Priority).FirstOrDefault();
                if (checkoutProvider != null && checkoutProvider.CheckoutRoute != null) {
                    return RedirectToRoute(checkoutProvider.CheckoutRoute);
                }
            }

            return Index();
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
            ShoppingCart cart = _shoppingCartService.BuildCart();

            return new ShapeResult(this, _shapeFactory.ShoppingCart_Widget()
                .Cart(cart)
                .NumberFormat(_currencyProvider.NumberFormat)
            );
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