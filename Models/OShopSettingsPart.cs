using Orchard.ContentManagement;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace OShop.Models {
    public class OShopSettingsPart : ContentPart, ICurrencySettings {
        // Currency
        [Required, MaxLength(3)]
        public string CurrencyIsoCode {
            get { return this.Retrieve(x => x.CurrencyIsoCode, "XXX"); }
            set { this.Store(x => x.CurrencyIsoCode, value); }
        }

        [Required]
        public string CurrencySymbol {
            get { return this.Retrieve(x => x.CurrencySymbol, "$"); }
            set { this.Store(x => x.CurrencySymbol, value); }
        }

        // NumberFormat
        [Required]
        public int CurrencyDecimalDigits {
            get { return this.Retrieve(x => x.CurrencyDecimalDigits, 2); }
            set { this.Store(x => x.CurrencyDecimalDigits, value); }
        }

        [Required]
        public int CurrencyNumberFormat {
            get { return this.Retrieve(x => x.CurrencyNumberFormat, 0); }
            set { this.Store(x => x.CurrencyNumberFormat, value); }
        }

        public string CurrencyDecimalSeparator {
            get { return NumberFormats.Formats[CurrencyNumberFormat].CurrencyDecimalSeparator; }
        }

        public string CurrencyGroupSeparator {
            get { return NumberFormats.Formats[CurrencyNumberFormat].CurrencyGroupSeparator; }
        }

        public int[] CurrencyGroupSizes {
            get { return NumberFormats.Formats[CurrencyNumberFormat].CurrencyGroupSizes; }
        }

        public int CurrencyNegativePattern {
            get { return this.Retrieve(x => x.CurrencyNegativePattern, 0); }
            set { this.Store(x => x.CurrencyNegativePattern, value); }
        }

        public int CurrencyPositivePattern {
            get { return this.Retrieve(x => x.CurrencyPositivePattern, 0); }
            set { this.Store(x => x.CurrencyPositivePattern, value); }
        }
    }
}