using Orchard;
using System;
using System.Globalization;
using Orchard.ContentManagement;
using OShop.Models;
using System.Collections.Generic;

namespace OShop.Services {
    public class DefaultCurrencyProvider : ICurrencyProvider {
        private OShopSettingsPart _settings;
        private NumberFormatInfo _numberFormat;

        // Converts currency negative pattern to matching number negative pattern
        // https://msdn.microsoft.com/fr-fr/library/system.globalization.numberformatinfo.currencynegativepattern(v=vs.100).aspx
        // https://msdn.microsoft.com/fr-fr/library/system.globalization.numberformatinfo.numbernegativepattern(v=vs.100).aspx
        private static readonly Dictionary<int, int> NegativePatternMatching = new Dictionary<int, int>() {
            {0, 0},
            {1, 1},
            {2, 1},
            {3, 3},
            {4, 0},
            {5, 1},
            {6, 3},
            {7, 3},
            {8, 1},
            {9, 2},
            {10, 4},
            {11, 3},
            {12, 1},
            {13, 3},
            {14, 0},
            {15, 0}
        };

        private OShopSettingsPart Settings {
            get {
                if (_settings == null) {
                    _settings = Services.WorkContext.CurrentSite.As<OShopSettingsPart>();
                }
                return _settings;
            }
        }

        public DefaultCurrencyProvider(IOrchardServices services) {
            Services = services;
        }

        public IOrchardServices Services { get; set; }

        public string IsoCode {
            get { return Settings.CurrencyIsoCode; }
        }

        public string Symbol {
            get { return NumberFormat.CurrencySymbol; }
        }

        public NumberFormatInfo NumberFormat {
            get {
                if(_numberFormat == null) {
                    _numberFormat = BuildNumberFormat(Settings);
                }
                return _numberFormat;
            }
        }

        public static NumberFormatInfo BuildNumberFormat(ICurrencySettings CurrencySettings) {
            var numberFormat = new NumberFormatInfo();
            numberFormat.CurrencySymbol = CurrencySettings.CurrencySymbol;
            numberFormat.CurrencyDecimalDigits = CurrencySettings.CurrencyDecimalDigits;
            numberFormat.NumberDecimalDigits = CurrencySettings.CurrencyDecimalDigits;
            numberFormat.CurrencyDecimalSeparator = CurrencySettings.CurrencyDecimalSeparator;
            numberFormat.NumberDecimalSeparator = CurrencySettings.CurrencyDecimalSeparator;
            numberFormat.CurrencyGroupSeparator = CurrencySettings.CurrencyGroupSeparator;
            numberFormat.NumberGroupSeparator = CurrencySettings.CurrencyGroupSeparator;
            numberFormat.CurrencyGroupSizes = CurrencySettings.CurrencyGroupSizes;
            numberFormat.NumberGroupSizes = CurrencySettings.CurrencyGroupSizes;
            numberFormat.CurrencyNegativePattern = CurrencySettings.CurrencyNegativePattern;
            numberFormat.NumberNegativePattern = NegativePatternMatching[CurrencySettings.CurrencyNegativePattern];
            numberFormat.CurrencyPositivePattern = CurrencySettings.CurrencyPositivePattern;

            return numberFormat;
        }
    }
}