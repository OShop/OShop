using OShop.Models;
using System.Collections.Generic;
using System.Linq;

namespace OShop.Helpers {
    public static class ShoppingCartExtensions {
        // TODO: Don't count items like discount or voucher
        public static int ItemsCount(this IEnumerable<ShoppingCartItem> Items) {
            return Items.Count();
        }

        public static decimal Total(this IEnumerable<ShoppingCartItem> Items) {
            return Items.Sum(ci => ci.SubTotal());
        }
    }
}