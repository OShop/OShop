using Orchard.Localization;
using Orchard.ContentManagement;
using OShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OShop.Extensions {
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

        public static VatRatePart GetVatRate(this IContent content) {
            if (content != null) {
                var vatPart = content.As<VatPart>();
                return vatPart != null ? vatPart.VatRate : null;
            }
            else {
                return null;
            }
        }

        public static decimal VatAmount(this IContent content) {
            var price = content.ContentItem.Parts.Cast<IPrice>().FirstOrDefault();
            return price != null ? content.VatAmount(price.Price) : 0;
        }

        public static decimal VatAmount(this IContent content, decimal Price) {
            return content.GetVatRate().TaxAmount(Price);
        }

        public static decimal VatIncluded(this IContent content) {
            var price = content.ContentItem.Parts.Cast<IPrice>().FirstOrDefault();
            return price != null ? content.VatIncluded(price.Price) : 0;
        }

        public static decimal VatIncluded(this IContent content, decimal Price) {
            return content.GetVatRate().TaxIncluded(Price);
        }

    }
}