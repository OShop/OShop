using Orchard.Localization;
using OShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OShop.Helpers {
    public static class VatExtensions {
        public static LocalizedString DisplayName(this VatRecord record, Localizer T) {
            return T("{0} ({1:P})", record.Name, record.Rate);
        }

        public static IEnumerable<SelectListItem> BuildVatSelectList(this IEnumerable<VatRecord> VatRecords, bool CreateEmpry = false, string EmptyString = "") {
            var result = new List<SelectListItem>();

            if (CreateEmpry) {
                result.Add(new SelectListItem() {
                    Value = "0",
                    Text = EmptyString
                });
            }
            result.AddRange(
                VatRecords.Select(v => new SelectListItem() {
                    Value = v.Id.ToString(),
                    Text = v.DisplayName(NullLocalizer.Instance).Text
                })
            );

            return result;
        }

        public static decimal GetVat(this VatRecord record, decimal price) {
            return record != null ? price * record.Rate : 0;
        }

        public static decimal GetVatIncludedPrice(this VatRecord record, decimal price) {
            return price + GetVat(record, price);
        }

        public static decimal VatIncludedUnitPrice(this ShoppingCartItem cartItem) {
            return cartItem.Item.VAT.GetVatIncludedPrice(cartItem.UnitPrice);
        }

        public static decimal VatIncludedSubTotal(this ShoppingCartItem cartItem) {
            return cartItem.Item.VAT.GetVatIncludedPrice(cartItem.SubTotal());
        }

        public static decimal SubTotalVat(this ShoppingCartItem cartItem) {
            return cartItem.Item.VAT.GetVat(cartItem.SubTotal());
        }

        public static decimal ItemsTotalVat(this ShoppingCart Cart) {
            return Cart.Items.Sum(ci => ci.SubTotalVat());
        }

        public static decimal VatIncludedItemsTotal(this ShoppingCart Cart) {
            return Cart.Items.Sum(ci => ci.VatIncludedSubTotal());
        }

        public static decimal CartTotalVat(this ShoppingCart Cart) {
            var shippingOption = Cart.Properties["ShippingOption"] as ShippingProviderOption;
            if (shippingOption != null) {
                return Cart.ItemsTotalVat() + shippingOption.Provider.VAT.GetVat(shippingOption.Option.Price);
            }
            else {
                return Cart.ItemsTotalVat();
            }
        }

        public static decimal VatIncludedCartTotal(this ShoppingCart Cart) {
            var shippingOption = Cart.Properties["ShippingOption"] as ShippingProviderOption;
            if (shippingOption != null) {
                return Cart.VatIncludedItemsTotal() + shippingOption.Provider.VAT.GetVatIncludedPrice(shippingOption.Option.Price);
            }
            else {
                return Cart.VatIncludedItemsTotal();
            }
        }
    }
}