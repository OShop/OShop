using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using Orchard;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingInfoResolver : IShoppingCartBuilder, IOrderBuilder {
        private readonly IContentManager _contentManager;
        private readonly IWorkContextAccessor _workContextAccessor;

        public ShippingInfoResolver(
            IContentManager contentManager,
            IWorkContextAccessor workContextAccessor) {
            _contentManager = contentManager;
            _workContextAccessor = workContextAccessor;
        }

        public Int32 Priority {
            get { return 50; }
        }

        public void BuildCart(IShoppingCartService ShoppingCartService, ref ShoppingCart Cart) {
            var cartRecords = ShoppingCartService.ListItems();
            var shippingParts = ListShippingParts(cartRecords);

            if (shippingParts.Any()) {
                var shippingInfos = Cart.Properties["ShippingInfos"] as IList<Tuple<int, IShippingInfo>> ?? new List<Tuple<int, IShippingInfo>>();
                foreach (var cartRecord in cartRecords) {
                    var shippingPart = shippingParts.Where(p => p.Id == cartRecord.ItemId).FirstOrDefault();

                    if (shippingPart != null) {
                        shippingInfos.Add(new Tuple<int, IShippingInfo>(cartRecord.Quantity, shippingPart));
                    }
                }
                Cart.Properties["ShippingInfos"] = shippingInfos;
            }
        }

        public void BuildOrder(IShoppingCartService ShoppingCartService, ref IContent Order) {
            var cartRecords = ShoppingCartService.ListItems();
            var shippingParts = ListShippingParts(cartRecords);

            if (shippingParts.Any()) {
                var workContext = _workContextAccessor.GetContext();
                var shippingInfos = workContext.GetState<IList<Tuple<int, IShippingInfo>>>("OShop.Orders.ShippingInfos") ?? new List<Tuple<int, IShippingInfo>>();
                foreach (var cartRecord in cartRecords) {
                    var shippingPart = shippingParts.Where(p => p.Id == cartRecord.ItemId).FirstOrDefault();

                    if (shippingPart != null) {
                        shippingInfos.Add(new Tuple<int, IShippingInfo>(cartRecord.Quantity, shippingPart));
                    }
                }
                workContext.SetState("OShop.Orders.ShippingInfos", shippingInfos);
            }
        }

        private IEnumerable<ShippingPart> ListShippingParts(IEnumerable<ShoppingCartItemRecord> cartRecords) {
            var shippingParts = _contentManager.GetMany<ShippingPart>(
                cartRecords.Select(cr => cr.ItemId),
                VersionOptions.Published,
                QueryHints.Empty);
            return shippingParts.Where(sp => sp.RequiresShipping);
        }

    }
}