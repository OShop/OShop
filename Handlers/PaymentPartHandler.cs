using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using OShop.Models;
using OShop.Services;
using System;
using System.Linq;

namespace OShop.Handlers {
    [OrchardFeature("OShop.Payment")]
    public class PaymentPartHandler : ContentHandler {
        public PaymentPartHandler(
            IRepository<PaymentPartRecord> repository,
            ICurrencyProvider currencyProvider
            ) {
            Filters.Add(StorageFilter.For(repository));

            OnActivated<PaymentPart>((context, part) => {
                part._payable.Loader(() => {
                    var payable = part.As<IPayable>();
                    return payable != null ? Math.Round(payable.PayableAmount, currencyProvider.NumberFormat.CurrencyDecimalDigits) : 0;
                });
                part._paid.Loader(() => Math.Round(
                    part.Transactions.Where(t => t.Status >= TransactionStatus.Validated).Sum(t => t.Amount),
                    currencyProvider.NumberFormat.CurrencyDecimalDigits)
                );
            });
        }
    }
}