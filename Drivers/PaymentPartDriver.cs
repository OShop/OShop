using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.ViewModels;
using Orchard.Environment.Extensions;
using Orchard.Localization.Services;
using Orchard.Services;
using OShop.Models;
using OShop.Services;
using OShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Payment")]
    public class PaymentPartDriver : ContentPartDriver<PaymentPart> {
        private readonly IClock _clock;
        private readonly IDateServices _dateServices;
        private readonly IPaymentService _paymentService;

        private const string TemplateName = "Parts/Payment";

        public PaymentPartDriver(
            IClock clock,
            IDateServices dateServices,
            IPaymentService paymentService,
            IOrchardServices services) {
            _clock = clock;
            _dateServices = dateServices;
            _paymentService = paymentService;
            Services = services;
        }

        public IOrchardServices Services { get; set; }

        protected override string Prefix { get { return "Payment"; } }

        protected override DriverResult Display(PaymentPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_Payment", () => shapeHelper.Parts_Payment(
                ContentPartDriver: part
            ));
        }

        protected override DriverResult Editor(PaymentPart part, dynamic shapeHelper) {
            var model = new PaymentEditViewModel() {
                AmountPaid = part.AmountPaid,
                PayableAmount = part.PayableAmount,
                Transactions = part.Transactions.Select(t => new PaymentTransactionEditViewModel() {
                    Id = t.Id,
                    Date = new DateTimeEditor() {
                        ShowDate = true,
                        ShowTime = true,
                        Date = _dateServices.ConvertToLocalDateString(t.Date),
                        Time = _dateServices.ConvertToLocalTimeString(t.Date),
                    },
                    Method = t.Method,
                    TransactionId = t.TransactionId,
                    Status = t.Status,
                    Amount = t.Amount
                }).ToArray(),
                NewTransaction = new PaymentTransactionEditViewModel() {
                    Date = new DateTimeEditor() {
                        ShowDate = true,
                        ShowTime = true,
                        Date = _dateServices.ConvertToLocalDateString(_clock.UtcNow),
                        Time = _dateServices.ConvertToLocalTimeString(_clock.UtcNow),
                    },
                    Status = TransactionStatus.Validated,
                    Amount = part.PayableAmount - part.AmountPaid
                }
            };

            return ContentShape("Parts_Payment_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: model,
                    Prefix: Prefix)
            );
        }

        protected override DriverResult Editor(PaymentPart part, IUpdateModel updater, dynamic shapeHelper) {
            if (Services.Authorizer.Authorize(Permissions.PaymentPermissions.ManagePayments)) {
                var model = new PaymentEditViewModel();
                if (updater.TryUpdateModel(model, Prefix, null, null)) {
                    // New transaction
                    if (model.NewTransaction != null && model.NewTransaction.Date != null) {
                        var date = _dateServices.ConvertFromLocalString(model.NewTransaction.Date.Date, model.NewTransaction.Date.Time);
                        if (date.HasValue
                            && !String.IsNullOrWhiteSpace(model.NewTransaction.Method)
                            && model.NewTransaction.Amount != 0) {
                            _paymentService.AddTransaction(part, model.NewTransaction.Method, model.NewTransaction.Amount, model.NewTransaction.TransactionId, model.NewTransaction.Status, date);
                        }
                    }
                    if (model.Transactions != null) {
                        // Updated transactions
                        foreach (var transaction in model.Transactions.Where(t => t.IsUpdated)) {
                            var date = _dateServices.ConvertFromLocalString(model.NewTransaction.Date.Date, model.NewTransaction.Date.Time);
                            _paymentService.UpdateTransaction(
                                transaction.Id,
                                Method: transaction.Method,
                                Amount: transaction.Amount,
                                TransactionId: transaction.TransactionId,
                                Status: transaction.Status,
                                Date: date);
                        }
                    }
                }
            }
            return Editor(part, shapeHelper);
        }

    }
}