using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;
using System.Collections.Generic;

namespace OShop.Models {
    public class PaymentPart : ContentPart<PaymentPartRecord> {
        internal readonly LazyField<decimal> _paid = new LazyField<decimal>();
        internal readonly LazyField<decimal> _payable = new LazyField<decimal>();

        public IEnumerable<PaymentTransactionRecord> Transactions {
            get { return Record.Transactions; }
        }

        public decimal AmountPaid {
            get { return _paid.Value; }
        }

        public decimal PayableAmount {
            get { return _payable.Value; }
        }

        public string Reference {
            get {
                var payable = this.As<IPayable>();
                return payable != null ? payable.Reference : "";
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