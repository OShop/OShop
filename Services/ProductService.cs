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
            foreach (var cartRecord in CartRecords.Where(cr=>cr.ItemType == ProductPart.PartItemType)) {
                var product = _contentManager.Get<ProductPart>(cartRecord.ItemId, VersionOptions.Published);

                if (product != null) {
                    CartItems.Add(new ShoppingCartItem {
                        Item = product,
                        Quantity = cartRecord.Quantity
                    });
                }
            }


        }
    }
}