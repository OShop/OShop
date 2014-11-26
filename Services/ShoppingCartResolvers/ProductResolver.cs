using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services.ShoppingCartResolvers {
    [OrchardFeature("OShop.Products")]
    public class ProductResolver : IShoppingCartBuilder {
        private readonly IContentManager _contentManager;

        public ProductResolver(
            IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public Int32 Priority {
            get { return 100; }
        }

        public void BuildCart(IShoppingCartService ShoppingCartService, ref ShoppingCart Cart) {
            var cartRecords = ShoppingCartService.ListItems();
            var products = _contentManager.GetMany<ProductPart>(
                cartRecords.Where(cr => cr.ItemType == ProductPart.PartItemType).Select(cr => cr.ItemId),
                VersionOptions.Published,
                QueryHints.Empty);

            foreach (var cartRecord in cartRecords.Where(cr => cr.ItemType == ProductPart.PartItemType)) {
                var product = products.Where(p => p.Id == cartRecord.ItemId).FirstOrDefault();

                if (product != null) {
                    Cart.Items.Add(new ShoppingCartItem {
                        Id = cartRecord.Id,
                        Item = product,
                        Quantity = cartRecord.Quantity
                    });
                }
            }
        }
    }
}