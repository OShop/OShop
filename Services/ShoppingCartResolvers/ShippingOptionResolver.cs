using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Environment.Extensions;
using OShop.Helpers;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingOptionResolver : IShoppingCartBuilder, IOrderBuilder {
        private readonly IShippingService _shippingService;
        private readonly IWorkContextAccessor _workContextAccessor;

        public ShippingOptionResolver(
            IShippingService shippingService,
            IWorkContextAccessor workContextAccessor) {
            _shippingService = shippingService;
            _workContextAccessor = workContextAccessor;
        }

        public Int32 Priority {
            get { return 200; }
        }

        public void BuildCart(IShoppingCartService ShoppingCartService, ShoppingCart Cart) {
            if (!Cart.IsShippingRequired()) {
                Cart.Shipping = null;
                return;
            }

            var zone = Cart.Properties["ShippingZone"] as ShippingZoneRecord;
            if (zone == null) {
                // Need a shipping zone
                //Cart.InvalidCart();
                Cart.Shipping = null;
                return;
            }

            var suitableProviders = _shippingService.GetSuitableProviderOptions(
                zone,
                Cart.Properties["ShippingInfos"] as IList<Tuple<int, IShippingInfo>> ?? new List<Tuple<int, IShippingInfo>>(),
                Cart.ItemsTotal()
            );

            if (!suitableProviders.Any()) {
                // Need a suitable shipping provider
                //Cart.InvalidCart();
                Cart.Shipping = null;
                return;
            }

            Int32 selectedProviderId = ShoppingCartService.GetProperty<int>("ShippingProviderId");
            var selectedProvider = suitableProviders.Where(p => p.Provider.Id == selectedProviderId).FirstOrDefault();
            if (selectedProvider != null) {
                // Apply selected provider
                Cart.Shipping = selectedProvider;
            }
            else {
                // Set cheapest option
                Cart.Shipping = suitableProviders.OrderBy(p => p.Option.Price).FirstOrDefault();
            }

        }

        public void BuildOrder(IShoppingCartService ShoppingCartService, IContent Order) {
            var orderPart = Order.As<OrderPart>();
            var shippingPart = Order.As<OrderShippingPart>();

            if (orderPart != null && shippingPart != null) {
                //  Shipping option
                var workContext = _workContextAccessor.GetContext();
                var shippingInfos = workContext.GetState<IList<Tuple<int, IShippingInfo>>>("OShop.Orders.ShippingInfos");
                if (!shippingInfos.IsShippingRequired()) {
                    shippingPart.ShippingStatus = ShippingStatus.NotRequired;
                    return;
                }

                Int32 selectedProviderId = ShoppingCartService.GetProperty<int>("ShippingProviderId");

                var suitableProviders = _shippingService.GetSuitableProviderOptions(
                    workContext.GetState<ShippingZoneRecord>("OShop.Orders.ShippingZone"),
                    workContext.GetState<IList<Tuple<int, IShippingInfo>>>("OShop.Orders.ShippingInfos") ?? new List<Tuple<int, IShippingInfo>>(),
                    orderPart.Items.Sum(i => i.UnitPrice * i.Quantity)
                );

                var selectedOption = suitableProviders.Where(po => po.Provider.Id == selectedProviderId).FirstOrDefault();
                if (selectedOption != null) {
                    shippingPart.ShippingInfos = new OrderShippingInfos {
                        Designation = selectedOption.Provider.As<ITitleAspect>().Title,
                        Description = selectedOption.Option.Name,
                        Price = selectedOption.Option.Price
                    };
                    shippingPart.ShippingStatus = ShippingStatus.Pending;
                }
            }
        }
    }
}