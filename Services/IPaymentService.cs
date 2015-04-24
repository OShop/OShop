using Orchard;
using OShop.Models;
using System;

namespace OShop.Services {
    public interface IPaymentService : IDependency {
        void AddTransaction(PaymentPart Part, string Method, decimal Amount, string TransactionId = "", TransactionStatus Status = TransactionStatus.Pending, DateTime? Date = null);
        void UpdateTransaction(int Id, string Method = null, decimal? Amount = null, string TransactionId = null, TransactionStatus? Status = null, DateTime? Date = null);
    }
}
