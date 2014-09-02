using OShop.Models;
using System.Globalization;
using System;

namespace OShop.Helpers {
    public static class VatExtensions {
        public static String DisplayName(this VatRecord record, IFormatProvider provider) {
            return String.Format(provider, "{0} ({1:P})", record.Name, record.Rate);
        }

        public static decimal GetVat(this VatRecord record, decimal price) {
            return price * record.Rate;
        }

        public static decimal GetVatIncludedPrice(this VatRecord record, decimal price) {
            return price + GetVat(record, price);
        }
    }
}