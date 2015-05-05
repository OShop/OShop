using Orchard;
using OShop.Models;
using System;

namespace OShop.Services {
    public interface IPaymentService : IDependency {
        PaymentTransactionRecord GetTransaction(int Id);
        void AddTransaction(PaymentPart Part, PaymentTransactionRecord Transaction);
        void UpdateTransaction(PaymentTransactionRecord Transaction);
    }
}
