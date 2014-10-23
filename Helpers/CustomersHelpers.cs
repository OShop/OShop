using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OShop.Helpers {
    public static class CustomersHelpers {
        public static IEnumerable<SelectListItem> BuildAddressSelectList(this IEnumerable<CustomerAddressPart> AddressRecords, bool CreateEmpry = false, string EmptyString = "") {
            var result = new List<SelectListItem>();

            if (CreateEmpry) {
                result.Add(new SelectListItem() {
                    Value = "0",
                    Text = EmptyString
                });
            }
            result.AddRange(
                AddressRecords.Select(z => new SelectListItem() {
                    Value = z.Id.ToString(),
                    Text = z.AddressAlias
                })
            );

            return result;
        }
    }
}