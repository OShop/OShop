using Orchard.Core.Common.ViewModels;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class PaymentEditViewModel {
        private PaymentPart _part;
        public PaymentEditViewModel() { }

        public PaymentEditViewModel(PaymentPart Part) {
            _part = Part;
        }

        public IEnumerable<PaymentTransactionRecord> Transactions {
            get {
                return _part != null ? _part.Transactions : new List<PaymentTransactionRecord>();
            }
        }

        public decimal AmountPaid {
            get {
                return _part != null ? _part.AmountPaid : 0;
            }
        }

        public decimal PayableAmount {
            get {
                return _part != null ? _part.PayableAmount : 0;
            }
        }

        public PaymentTransactionEditViewModel NewTransaction { get; set; }
    }

    public class PaymentTransactionEditViewModel {
        public DateTimeEditor Date { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public string TransactionId { get; set; }
    }
}