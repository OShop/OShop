using Orchard.ContentManagement;
using OShop.Helpers;

namespace OShop.Models {
    public class ShoppingCartItem {
        public int Id { get; set; }
        public IShopItem Item;
        public int Quantity;

        public decimal UnitPrice {
            get {
                return Item.GetUnitPrice(Quantity);
            }
        }
    }
}