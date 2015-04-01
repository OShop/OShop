using OShop.Models;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Helpers {
    public static class TaxExtensions {
        public static decimal TaxAmount(this ITax tax, decimal Price) {
            return tax != null ? tax.Rate * Price : 0;
        }

        public static decimal TaxIncluded(this ITax tax, decimal Price) {
            return Price + (tax != null ? tax.Rate * Price : 0);
        }

        public static void AddTax(this IList<TaxAmount> amounts, ITax tax, decimal taxBase) {
            var taxAmount = amounts.Where(ta => ta.Tax.Name == tax.Name && ta.Tax.Rate == tax.Rate).FirstOrDefault();
            if (taxAmount == null && taxBase != 0) {
                taxAmount = new TaxAmount(tax);
                amounts.Add(taxAmount);
            }
            taxAmount.TaxBase += taxBase;
        }

    }
}