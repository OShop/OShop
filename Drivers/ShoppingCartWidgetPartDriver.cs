using Orchard;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.Environment.Features;
using OShop.Models;
using OShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Drivers {
    [OrchardFeature("OShop.ShoppingCart")]
    public class ShoppingCartWidgetPartDriver : ContentPartDriver<ShoppingCartWidgetPart> {
        private readonly IEnumerable<IShoppingCartResolver> _shoppingCartResolvers;
        private readonly ICurrencyProvider _currencyProvider;
        private readonly IFeatureManager _featureManager;

        public ShoppingCartWidgetPartDriver(
            IEnumerable<IShoppingCartResolver> shoppingCartResolvers,
            ICurrencyProvider currencyProvider,
            IFeatureManager featureManager) {
            _shoppingCartResolvers = shoppingCartResolvers;
            _currencyProvider = currencyProvider;
            _featureManager = featureManager;
        }

        protected override DriverResult Display(ShoppingCartWidgetPart part, string displayType, dynamic shapeHelper) {
            ShoppingCart cart = new ShoppingCart();
            foreach (var resolver in _shoppingCartResolvers.OrderByDescending(r => r.Priority)) {
                resolver.ResolveCart(ref cart);
            }

            return ContentShape("Parts_ShoppingCartWidget", () => shapeHelper.Parts_ShoppingCartWidget(
                Cart: cart,
                NumberFormat: _currencyProvider.NumberFormat,
                VatEnabled: _featureManager.GetEnabledFeatures().Where(f => f.Id == "OShop.VAT").Any(),
                ContentItem: part.ContentItem
            ));
        }
    }
}