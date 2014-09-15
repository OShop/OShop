using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class ShoppingCartItem {
        public int Id;
        public IShopItem Item;
        public int Quantity;

        public decimal UnitPrice() {
            return Item.GetUnitPrice(Quantity);
        }

        public decimal SubTotal() {
            return UnitPrice() * Quantity;
        }
    }
}