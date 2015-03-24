using Orchard.Localization;
using Orchard.ContentManagement;
using OShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OShop.Helpers {
    public static class VatExtensions {
        public static LocalizedString DisplayName(this VatRatePart part, Localizer T) {
            return T("{0} ({1:P})", part.Name, part.Rate);
        }

        public static IEnumerable<SelectListItem> BuildVatSelectList(this IEnumerable<VatRatePart> VatRecords, bool CreateEmpry = false, string EmptyString = "") {
            var result = new List<SelectListItem>();

            if (CreateEmpry) {
                result.Add(new SelectListItem() {
                    Value = "0",
                    Text = EmptyString
                });
            }
            result.AddRange(
                VatRecords.Select(v => new SelectListItem() {
                    Value = v.IsPublished() ? v.Id.ToString() : "",
                    Text = v.DisplayName(NullLocalizer.Instance).Text
                })
            );

            return result;
        }

        public static decimal GetVat(this VatRatePart part, decimal price) {
            return part != null ? price * part.Rate : 0;
        }

        public static decimal GetVatIncludedPrice(this VatRatePart part, decimal price) {
            return price + GetVat(part, price);
        }

        public static VatRatePart GetVatRate(this IContent content) {
            if (content != null) {
                var vatPart = content.As<VatPart>();
                return vatPart != null ? vatPart.Rate : null;
            }
            else {
                return null;
            }
        }

        public static decimal VatIncludedUnitPrice(this ShoppingCartItem cartItem) {
            return cartItem.Item.GetVatRate().GetVatIncludedPrice(cartItem.UnitPrice);
        }

        public static decimal VatIncludedSubTotal(this ShoppingCartItem cartItem) {
            return cartItem.Item.GetVatRate().GetVatIncludedPrice(cartItem.SubTotal());
        }

        public static decimal SubTotalVat(this ShoppingCartItem cartItem) {
            return cartItem.Item.GetVatRate().GetVat(cartItem.SubTotal());
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
                return Cart.ItemsTotalVat() + shippingOption.Provider.GetVatRate().GetVat(shippingOption.Option.Price);
            }
            else {
                return Cart.ItemsTotalVat();
            }
        }

        public static decimal VatIncludedCartTotal(this ShoppingCart Cart) {
            var shippingOption = Cart.Properties["ShippingOption"] as ShippingProviderOption;
            if (shippingOption != null) {
                return Cart.VatIncludedItemsTotal() + shippingOption.Provider.GetVatRate().GetVatIncludedPrice(shippingOption.Option.Price);
            }
            else {
                return Cart.VatIncludedItemsTotal();
            }
        }
    }
}