using Orchard;
using System;
using System.Globalization;
using Orchard.ContentManagement;
using OShop.Models;

namespace OShop.Services {
    public class DefaultCurrencyProvider : ICurrencyProvider {
        private OShopSettingsPart _settings;
        private NumberFormatInfo _numberFormat;

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
            numberFormat.CurrencyDecimalSeparator = CurrencySettings.CurrencyDecimalSeparator;
            numberFormat.CurrencyGroupSeparator = CurrencySettings.CurrencyGroupSeparator;
            numberFormat.CurrencyGroupSizes = CurrencySettings.CurrencyGroupSizes;
            numberFormat.CurrencyNegativePattern = CurrencySettings.CurrencyNegativePattern;
            numberFormat.CurrencyPositivePattern = CurrencySettings.CurrencyPositivePattern;

            return numberFormat;
        }
    }
}