using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class ShoppingCartItem {
        public IShopItem Item;
        public int Quantity;

        public decimal UnitPrice {
            get {
                return Item.GetUnitPrice(Quantity);
            }
        }

        public decimal SubTotal {
            get {
                return UnitPrice * Quantity;
            }
        }
    }
}