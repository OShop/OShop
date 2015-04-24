using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class PaymentPart : ContentPart<PaymentPartRecord> {
        public IEnumerable<PaymentTransactionRecord> Transactions {
            get { return Record.Transactions; }
        }

        public decimal AmountPaid {
            get { return Transactions.Where(t => t.Status >= TransactionStatus.Validated).Sum(t => t.Amount); }
        }

        public decimal PayableAmount {
            get {
                var payable = this.As<IPayable>();
                return payable != null ? payable.PayableAmount : 0;
            }
        }

        public PaymentStatus Status {
            get {
                if (AmountPaid >= PayableAmount) {
                    return PaymentStatus.Completed;
                }
                else if (AmountPaid > 0) {
                    return PaymentStatus.Partial;
                }
                else {
                    return PaymentStatus.Awaiting;
                }
            }
        }
    }

    public enum PaymentStatus : int {
        Awaiting = 0,
        Partial = 1,
        Completed = 2
    }
}