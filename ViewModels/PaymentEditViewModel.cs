using Orchard.Core.Common.ViewModels;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.ViewModels {
    public class PaymentEditViewModel {
        public decimal AmountPaid { get; set; }
        public decimal PayableAmount { get; set; }
        public PaymentTransactionEditViewModel[] Transactions { get; set; }
        public PaymentTransactionEditViewModel NewTransaction { get; set; }
    }

    public class PaymentTransactionEditViewModel {
        public int Id { get; set; }
        public bool IsUpdated { get; set; }
        public DateTimeEditor Date { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public string TransactionId { get; set; }
        public TransactionStatus Status { get; set; }
    }
}