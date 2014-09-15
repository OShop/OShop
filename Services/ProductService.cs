using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services {
    [OrchardFeature("OShop.Products")]
    public class ProductService : IShopItemProvider {
        private readonly IContentManager _contentManager;

        public ProductService(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public short Priority {
            get { return 0; }
        }

        public void GetItems(IEnumerable<ShoppingCartItemRecord> CartRecords, ref List<ShoppingCartItem> CartItems) {
            var products = _contentManager.GetMany<ProductPart>(
                CartRecords.Where(cr => cr.ItemType == ProductPart.PartItemType).Select(cr => cr.ItemId),
                VersionOptions.Published,
                QueryHints.Empty);

            foreach (var cartRecord in CartRecords.Where(cr=>cr.ItemType == ProductPart.PartItemType)) {
                var product = products.Where(p => p.Id == cartRecord.ItemId).FirstOrDefault();

                if (product != null) {
                    CartItems.Add(new ShoppingCartItem {
                        Id = cartRecord.Id,
                        Item = product,
                        Quantity = cartRecord.Quantity
                    });
                }
            }


        }
    }
}