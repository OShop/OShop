using Orchard.ContentManagement;
using OShop.Helpers;

namespace OShop.Models {
    public class ShoppingCartItem : IPrice {
        public int Id { get; set; }
        public IShopItem Item;
        public int Quantity;

        public decimal UnitPrice {
            get {
                return Item.GetUnitPrice(Quantity);
            }
        }

        public decimal Price {
            get { return this.SubTotal(); }
        }

        public ContentItem ContentItem {
            get { return Item.ContentItem; }
        }
    }
}