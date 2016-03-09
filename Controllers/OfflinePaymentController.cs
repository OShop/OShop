using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc.Html;
using Orchard.Services;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using OShop.Models;
using OShop.Permissions;
using OShop.Services;
using System.Web.Mvc;

namespace OShop.Controllers {
    [OrchardFeature("OShop.OfflinePayment")]
    public class OfflinePaymentController : Controller {
        private readonly IClock _clock;
        private readonly IPaymentService _paymentService;
        private readonly ICurrencyProvider _currencyProvider;

        public OfflinePaymentController(
            IOrchardServices services,
            IClock clock,
            IPaymentService paymentService,
            ICurrencyProvider currencyProvider) {
            Services = services;
            _clock = clock;
            _paymentService = paymentService;
            _currencyProvider = currencyProvider;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }

        // GET: OfflinePaymentSettings
        [Admin]
        public ActionResult Settings()
        {
            if (!Services.Authorizer.Authorize(OShopPermissions.ManageShopSettings, T("Not allowed to manage Offline Payment Settings")))
                return new HttpUnauthorizedResult();

            var offlinePaymentSettings = Services.WorkContext.CurrentSite.As<OfflinePaymentSettingsPart>();

            return View(offlinePaymentSettings);
        }

        [Admin]
        [HttpPost, ActionName("Settings")]
        public ActionResult SettingsPOST() {
            if (!Services.Authorizer.Authorize(OShopPermissions.ManageShopSettings, T("Not allowed to manage Offline Payment Settings")))
                return new HttpUnauthorizedResult();

            var offlinePaymentSettings = Services.WorkContext.CurrentSite.As<OfflinePaymentSettingsPart>();

            if (TryUpdateModel(offlinePaymentSettings)) {
                Services.Notifier.Information(T("Offline Payment Settings saved successfully."));
            }
            else {
                Services.Notifier.Error(T("Could not save Offline Payment Settings."));
            }

            return Settings();
        }

        /// <summary>
        /// Create offline payment for a ContentItem with a PaymentPart
        /// </summary>
        /// <param name="Id">ContentItem Id</param>
        public ActionResult Create(int Id) {
            var paymentPart = Services.ContentManager.Get<PaymentPart>(Id);
            if (paymentPart == null) {
                return new HttpNotFoundResult();
            }

            decimal OutstandingAmount = paymentPart.PayableAmount - paymentPart.AmountPaid;
            if (OutstandingAmount <= 0) {
                Services.Notifier.Information(T("Nothing left to pay on this document."));
                return Redirect(Url.ItemDisplayUrl(paymentPart));
            }

            var transaction = new PaymentTransactionRecord() {
                Method = "Offline",
                Amount = OutstandingAmount,
                Date = _clock.UtcNow,
                Status = TransactionStatus.Pending
            };

            _paymentService.AddTransaction(paymentPart, transaction);

            Services.Notifier.Information(T("Transaction reference : <b>{0}</b><br/>Transaction amount : <b>{1}</b>", paymentPart.Reference, OutstandingAmount.ToString("C", _currencyProvider.NumberFormat)));

            var offlinePaymentSettings = Services.WorkContext.CurrentSite.As<OfflinePaymentSettingsPart>();
            return Redirect(Url.ItemDisplayUrl(offlinePaymentSettings.Content));
        }
    }
}