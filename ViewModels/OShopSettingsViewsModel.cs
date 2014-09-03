using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class OShopSettingsViewsModel {
        // Currency
        public string CurrencyIsoCode { get; set; }
        public string CurrencySymbol { get; set; }

        // NumberFormat
        public int CurrencyDecimalDigits { get; set; }
        public int CurrencyNumberFormat { get; set; }
        public Dictionary<int, string> CurrencyNumberFormats { get; set; }
        public int CurrencyPositivePattern { get; set; }
        public Dictionary<int, string> CurrencyPositivePatterns { get; set; }
        public int CurrencyNegativePattern { get; set; }
        public Dictionary<int, string> CurrencyNegativePatterns { get; set; }
    }
}