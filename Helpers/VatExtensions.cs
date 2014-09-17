using OShop.Models;
using System.Globalization;
using System;
using Orchard.Localization;

namespace OShop.Helpers {
    public static class VatExtensions {
        public static LocalizedString DisplayName(this VatRecord record, Localizer T) {
            return T("{0} ({1:P})", record.Name, record.Rate);
        }

        public static decimal GetVat(this VatRecord record, decimal price) {
            return record != null ? price * record.Rate : 0;
        }

        public static decimal GetVatIncludedPrice(this VatRecord record, decimal price) {
            return price + GetVat(record, price);
        }

        public static decimal VatIncludedUnitPrice(this ShoppingCartItem cartItem) {
            return cartItem.Item.VAT.GetVatIncludedPrice(cartItem.UnitPrice());
        }

        public static decimal VatIncludedSubTotal(this ShoppingCartItem cartItem) {
            return cartItem.Item.VAT.GetVatIncludedPrice(cartItem.SubTotal());
        }

        public static decimal VatSubTotal(this ShoppingCartItem cartItem) {
            return cartItem.Item.VAT.GetVat(cartItem.SubTotal());
        }

    }
}