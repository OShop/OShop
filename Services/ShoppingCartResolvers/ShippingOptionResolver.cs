using Orchard.Environment.Extensions;
using OShop.Helpers;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingOptionResolver : IShoppingCartResolver {
        private readonly IShippingService _shippingService;
        private readonly IShoppingCartService _shoppingCartService;

        public ShippingOptionResolver(
            IShippingService shippingService,
            IShoppingCartService shoppingCartService) {
            _shippingService = shippingService;
            _shoppingCartService = shoppingCartService;

        }

        public Int32 Priority {
            get { return 10; }
        }

        public void ResolveCart(ref ShoppingCart Cart) {
            if (!Cart.IsShippingRequired()) {
                return;
            }

            if (Cart.ShippingZone == null) {
                // Need a shipping zone
                Cart.InvalidCart();
            }

            var suitableProviders = _shippingService.GetSuitableProviderOptions(Cart);

            if (!suitableProviders.Any()) {
                // Need a suitable shipping provider
                Cart.InvalidCart();
                return;
            }

            Int32 selectedProviderId = _shoppingCartService.GetProperty<int>("ShippingProviderId");
            var selectedProvider = suitableProviders.Where(p => p.Provider.Id == selectedProviderId).FirstOrDefault();
            if (selectedProvider != null) {
                // Apply selected provider
                Cart.ShippingOption = selectedProvider;
            }
            else {
                // Set cheapest option
                Cart.ShippingOption = suitableProviders.OrderBy(p => p.Option.Price).FirstOrDefault();
            }

        }
    }
}