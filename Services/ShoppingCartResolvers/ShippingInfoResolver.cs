using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Shipping")]
    public class ShippingInfoResolver : IShoppingCartBuilder {
        private readonly IContentManager _contentManager;

        public ShippingInfoResolver(
            IContentManager contentManager) {
            _contentManager = contentManager;
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

        private IEnumerable<ShippingPart> ListShippingParts(IEnumerable<ShoppingCartItemRecord> cartRecords) {
            var shippingParts = _contentManager.GetMany<ShippingPart>(
                cartRecords.Select(cr => cr.ItemId),
                VersionOptions.Published,
                QueryHints.Empty);
            return shippingParts.Where(sp => sp.RequiresShipping);
        }
    }
}