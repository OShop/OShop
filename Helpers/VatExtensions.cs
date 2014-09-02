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
            return price * record.Rate;
        }

        public static decimal GetVatIncludedPrice(this VatRecord record, decimal price) {
            return price + GetVat(record, price);
        }
    }
}