using Orchard.ContentManagement;
using OShop.Models;
using System;

namespace OShop.Services.ShoppingCartResolvers {
    public class StockResolver : IShoppingCartBuilder, IOrderBuilder {
        private readonly IContentManager _contentManager;

        public StockResolver(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public Int32 Priority {
            get { return 700; }
        }

        public void BuildCart(IShoppingCartService ShoppingCartService, ShoppingCart Cart) {
            LimitQuantities(ShoppingCartService);
        }

        public void BuildOrder(IShoppingCartService ShoppingCartService, IContent Order) {
            LimitQuantities(ShoppingCartService);
        }

        private void LimitQuantities(IShoppingCartService ShoppingCartService) {
            var cartRecords = ShoppingCartService.ListItems();

            foreach (var record in cartRecords) {
                var stock = _contentManager.Get(record.ItemId).As<IStock>();
                if (stock != null && stock.MaxOrderQty.HasValue) {
                    if (record.Quantity > stock.MaxOrderQty) {
                        ShoppingCartService.UpdateQuantity(record.Id, stock.MaxOrderQty.Value);
                    }
                }
            }
        }
    }
}