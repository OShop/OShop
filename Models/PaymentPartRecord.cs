using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class PaymentPartRecord : ContentPartRecord {
        public PaymentPartRecord() {
            Transactions = new List<PaymentTransactionRecord>();
        }

        public virtual IList<PaymentTransactionRecord> Transactions { get; set; }
    }
}