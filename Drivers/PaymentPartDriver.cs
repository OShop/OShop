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
        private readonly IEnumerable<IPaymentProvider> _paymentProviders;

        private const string TemplateName = "Parts/Payment";

        public PaymentPartDriver(
            IClock clock,
            IDateServices dateServices,
            IPaymentService paymentService,
            IEnumerable<IPaymentProvider> paymentProviders,
            IOrchardServices services) {
            _clock = clock;
            _dateServices = dateServices;
            _paymentService = paymentService;
            _paymentProviders = paymentProviders.OrderByDescending(p => p.Priority);
            Services = services;
        }

        public IOrchardServices Services { get; set; }

        protected override string Prefix { get { return "Payment"; } }

        protected override DriverResult Display(PaymentPart part, string displayType, dynamic shapeHelper) {
            bool showProviders = part.ContentItem.Id > 0 && _paymentProviders.Any() && part.Status < PaymentStatus.Completed;

            return Combined(
                ContentShape("Parts_Payment", () => shapeHelper.Parts_Payment(
                    ContentPartDriver: part
                )),
                showProviders ?
                ContentShape("Parts_Payment_Providers", () => shapeHelper.Parts_Payment_Providers(
                    ContentPartDriver: part,
                    Providers: _paymentProviders
                ))
                : null
            );
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
                                _paymentService.AddTransaction(part, new PaymentTransactionRecord() {
                                    Date = date.Value,
                                    Amount = model.NewTransaction.Amount,
                                    Method = model.NewTransaction.Method,
                                    TransactionId = model.NewTransaction.TransactionId,
                                    Status = model.NewTransaction.Status
                                });
                        }
                    }
                    if (model.Transactions != null) {
                        // Updated transactions
                        foreach (var transactionVM in model.Transactions.Where(t => t.IsUpdated)) {
                            var date = _dateServices.ConvertFromLocalString(model.NewTransaction.Date.Date, model.NewTransaction.Date.Time);
                            var transactionRecord = _paymentService.GetTransaction(transactionVM.Id);
                            if (transactionRecord != null) {
                                if (date.HasValue) {
                                    transactionRecord.Date = date.Value;
                                }
                                transactionRecord.Method = transactionVM.Method;
                                transactionRecord.Amount = transactionVM.Amount;
                                transactionRecord.TransactionId = transactionVM.TransactionId;
                                transactionRecord.Status = transactionVM.Status;
                            }
                        }
                    }
                }
            }
            return Editor(part, shapeHelper);
        }

    }
}