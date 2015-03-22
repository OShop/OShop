using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using OShop.Models;
using OShop.Permissions;
using OShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [Admin]
    public class SettingsController : Controller
    {
        public SettingsController(IOrchardServices services) {
            Services = services;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult Index()
        {
            if(!Services.Authorizer.Authorize(OShopPermissions.ManageShopSettings, T("Not allowed to manage Shop Settings")))
                return new HttpUnauthorizedResult();

            var settings = Services.WorkContext.CurrentSite.As<OShopSettingsPart>();

            var model = new OShopSettingsViewsModel {
                CurrencyIsoCode = settings.CurrencyIsoCode,
                CurrencySymbol = settings.CurrencySymbol,
                CurrencyDecimalDigits = settings.CurrencyDecimalDigits,
                CurrencyNumberFormat = settings.CurrencyNumberFormat,
                CurrencyNumberFormats = GetNumberFormats(),
                CurrencyNegativePattern = settings.CurrencyNegativePattern,
                CurrencyPositivePatterns = GetCurrencyPositivePatterns(),
                CurrencyPositivePattern = settings.CurrencyPositivePattern,
                CurrencyNegativePatterns = GetCurrencyNegativePatterns()
            };
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(OShopSettingsViewsModel model) {
            if (!Services.Authorizer.Authorize(OShopPermissions.ManageShopSettings, T("Not allowed to manage Shop Settings")))
                return new HttpUnauthorizedResult();

            var settings = Services.WorkContext.CurrentSite.As<OShopSettingsPart>();

            if (TryUpdateModel(settings)) {
                Services.Notifier.Information(T("OShop Settings saved successfully."));
            }
            else {
                Services.Notifier.Error(T("Could not save OShop Settings."));
            }

            return Index();
        }

        private Dictionary<int, string> GetNumberFormats() {
            Dictionary<int, string> result = new Dictionary<int, string>();

            NumberFormatInfo numberFormat = new NumberFormatInfo() {
                CurrencySymbol = ""
            };

            for (int i = 0; i < NumberFormats.Formats.Length; i++) {
                numberFormat.CurrencyDecimalSeparator = NumberFormats.Formats[i].CurrencyDecimalSeparator;
                numberFormat.CurrencyGroupSeparator = NumberFormats.Formats[i].CurrencyGroupSeparator;
                numberFormat.CurrencyGroupSizes = NumberFormats.Formats[i].CurrencyGroupSizes;

                result.Add(i, String.Format(numberFormat, "{0:C}", 1234.56));
            }

            return result;
        }

        private Dictionary<int, string> GetCurrencyPositivePatterns() {
            Dictionary<int, string> result = new Dictionary<int, string>();

            NumberFormatInfo numberFormat = new NumberFormatInfo() {
                CurrencyDecimalDigits = 0
            };

            for (int i = 0; i <= 3; i++) {
                numberFormat.CurrencyPositivePattern = i;

                result.Add(i, String.Format(numberFormat, "{0:C}", 123));
            }

            return result;
        }

        private Dictionary<int, string> GetCurrencyNegativePatterns() {
            Dictionary<int, string> result = new Dictionary<int, string>();

            NumberFormatInfo numberFormat = new NumberFormatInfo() {
                CurrencyDecimalDigits = 0
            };

            for (int i = 0; i <= 15; i++) {
                numberFormat.CurrencyNegativePattern = i;

                result.Add(i, String.Format(numberFormat, "{0:C}", -123));
            }

            return result;
        }

    }
}