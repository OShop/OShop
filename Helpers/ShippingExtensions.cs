using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OShop.Helpers {
    public static class ShippingExtensions {
        public static bool IsShippingRequired(this ShoppingCart Cart) {
            var shippingInfos = Cart.Properties["ShippingInfos"] as IList<Tuple<int, IShippingInfo>>;
            return shippingInfos.IsShippingRequired();
        }

        public static bool IsShippingRequired(this IList<Tuple<int, IShippingInfo>> ShippingInfos) {
            return ShippingInfos == null ? false : ShippingInfos.Any();
        }

        public static IEnumerable<SelectListItem> BuildZoneSelectList(this IEnumerable<ShippingZoneRecord> ZoneRecords, bool CreateEmpry = false, string EmptyString = "") {
            var result = new List<SelectListItem>();

            if (CreateEmpry) {
                result.Add(new SelectListItem() {
                    Value = "0",
                    Text = EmptyString
                });
            }
            result.AddRange(
                ZoneRecords.Select(z => new SelectListItem() {
                    Value = z.Id.ToString(),
                    Text = z.Name
                })
            );

            return result;
        }

    }
}