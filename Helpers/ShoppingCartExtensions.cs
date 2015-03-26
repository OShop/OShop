using OShop.Models;
using System.Collections.Generic;
using System.Linq;

namespace OShop.Helpers {
    public static class ShoppingCartExtensions {
        // TODO: Don't count items like discount or voucher
        public static int ItemsCount(this IEnumerable<ShoppingCartItem> Items) {
            return Items.Count();
        }

        public static decimal SubTotal(this ShoppingCartItem Item) {
            return Item.UnitPrice * Item.Quantity;
        }

        public static decimal ItemsTotal(this ShoppingCart Cart) {
            return Cart.Items.Sum(ci => ci.SubTotal());
        }

        public static decimal TaxesTotal(this ShoppingCart Cart) {
            return Cart.Taxes.Sum(t => t.Tax.Rate * t.TaxBase);
        }

        public static decimal CartTotal(this ShoppingCart Cart) {
            var shipping = Cart.Shipping as IPrice;
            return Cart.ItemsTotal() + (shipping != null ? shipping.Price : 0) + Cart.TaxesTotal();
        }
    }
}