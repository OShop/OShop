using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OShop.Helpers {
    public static class LocationsExtensions {
        public static IEnumerable<SelectListItem> BuildCountrySelectList(this IEnumerable<LocationsCountryRecord> ZoneRecords, bool CreateEmpry = false, string EmptyString = "") {
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