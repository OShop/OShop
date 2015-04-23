using Orchard;
using OShop.Models;
using System;

namespace OShop.Services {
    public interface IPaymentService : IDependency {
        void AddTransaction(PaymentPart part, string Method, decimal Amount, string TransactionId = "", DateTime? Date = null);
    }
}
