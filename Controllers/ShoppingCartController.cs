using Orchard.Environment.Extensions;
using Orchard.Environment.Features;
using Orchard.Themes;
using OShop.Models;
using OShop.Services;
using OShop.ViewModels;
using OShop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.ContentManagement;

namespace OShop.Controllers
{
    [OrchardFeature("OShop.ShoppingCart")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICurrencyProvider _currencyProvider;
        private readonly IEnumerable<IShoppingCartResolver> _shoppingCartResolvers;
        private readonly IFeatureManager _featureManager;
        private readonly IContentManager _contentManager;

        private readonly ILocationsService _locationService;
        private readonly IShippingService _shippingService;

        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            ICurrencyProvider currencyProvider,
            IEnumerable<IShoppingCartResolver> shoppingCartResolvers,
            IFeatureManager featureManager,
            IContentManager contentManager,
            ILocationsService locationService = null,
            IShippingService shippingService = null) {
            _shoppingCartService = shoppingCartService;
            _currencyProvider = currencyProvider;
            _shoppingCartResolvers = shoppingCartResolvers;
            _featureManager = featureManager;
            _contentManager = contentManager;
            _locationService = locationService;
            _shippingService = shippingService;
        }

        [Themed]
        [OutputCache(Duration = 0)]
        public ActionResult Index()
        {
            Location location = new Location(null, null);
            ShippingZoneRecord shippingZone = null;

            if (_locationService != null) {
                location = GetLocation(
                    _shoppingCartService.GetProperty<int>("CountryId"),
                    _shoppingCartService.GetProperty<int>("StateId")
                );
            };

            if (_shippingService != null) {
                if (location.State != null && location.State.ShippingZoneRecord != null && location.State.ShippingZoneRecord.Enabled) {
                    shippingZone = location.State.ShippingZoneRecord;
                }
                else if (location.Country != null && location.Country.ShippingZoneRecord != null && location.Country.ShippingZoneRecord.Enabled) {
                    shippingZone = location.Country.ShippingZoneRecord;
                }
            }

            ShoppingCart cart = new ShoppingCart();
            foreach (var resolver in _shoppingCartResolvers.OrderByDescending(r => r.Priority)) {
                resolver.ResolveCart(ref cart);
            }

            var model = new ShoppingCartIndexViewModel() {
                CartItems = cart.Items,
                NumberFormat = _currencyProvider.NumberFormat,
                // Optional features
                VatEnabled = _featureManager.GetEnabledFeatures().Where(f => f.Id == "OShop.VAT").Any(),

                ShippingRequired = cart.IsShippingRequired()
            };

            if (_locationService != null) {
                model.CountryId = location.Country != null ? location.Country.Id : 0;
                model.StateId = location.State != null ? location.State.Id : 0;

                model.Countries = _locationService.GetEnabledCountries();
                model.States = _locationService.GetEnabledStates(model.CountryId);
            }

            if (_shippingService != null) {
                var publishedProviders = _contentManager.Query<ShippingProviderPart>(VersionOptions.Published).List();

                List<ShippingProviderWithOption> suitableProviders = new List<ShippingProviderWithOption>();
                foreach (var provider in publishedProviders) {
                    var option = _shippingService.GetSuitableOption(provider.Id, shippingZone, cart);
                    if (option != null) {
                        suitableProviders.Add(new ShippingProviderWithOption(provider, option));
                    }
                }

                model.ShippingProviders = suitableProviders.OrderBy(p => p.Option.Price);
                model.ShippingProviderId = _shoppingCartService.GetProperty<int>("ShippingProviderId");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Update(ShoppingCartUpdateViewModel model) {
            if (model.CartItems != null) {
                UpdateCart(model.CartItems);
            }

            if (_locationService != null) {
                var location = GetLocation(model.CountryId, model.StateId);

                if (location.Country != null) {
                    _shoppingCartService.SetProperty("CountryId", location.Country.Id);
                    if (location.State != null) {
                        _shoppingCartService.SetProperty("StateId", location.State.Id);
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

            return RedirectToAction("Index");
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

        private Location GetLocation(int CountryId, int StateId) {
            var country = _locationService.GetCountry(CountryId);

            if (country != null && country.Enabled) {
                var state = country.States.Where(s => s.Id == StateId && s.Enabled).FirstOrDefault();
                if (state != null) {
                    return new Location(country, state);
                }
                else {
                    return new Location(country, null);
                }
            }
            else {
                return new Location(null, null);
            }

        }

        private struct Location {
            public Location(LocationsCountryRecord Country, LocationsStateRecord State) {
                this.Country = Country;
                this.State = State;
            }

            public LocationsCountryRecord Country;
            public LocationsStateRecord State;
        }
    }
}