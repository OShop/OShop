using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class PaymentTransactionRecord {
        public virtual int Id { get; set; }
        public virtual PaymentPartRecord PaymentPartRecord { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual string Method { get; set; }
        public virtual string TransactionId { get; set; }
        public virtual TransactionStatus Status { get; set; }
    }

    public enum TransactionStatus {
        Canceled = -1,
        Pending = 0,
        Validated = 1
    }
}