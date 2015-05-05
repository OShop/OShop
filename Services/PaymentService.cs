using Orchard.Data;
using OShop.Models;
using System;

namespace OShop.Services {
    public class PaymentService : IPaymentService {
        private readonly IRepository<PaymentTransactionRecord> _transactionRepository;

        public PaymentService(
            IRepository<PaymentTransactionRecord> transactionRepository) {
            _transactionRepository = transactionRepository;
        }

        public PaymentTransactionRecord GetTransaction(int Id) {
            return _transactionRepository.Get(Id);
        }

        public void AddTransaction(PaymentPart Part, PaymentTransactionRecord Transaction) {
            if (Part == null) {
                throw new ArgumentNullException("Part", "PaymentPart cannot be null.");
            }
            if (Transaction == null) {
                throw new ArgumentNullException("Transaction", "Transaction cannot be null.");
            }
            Transaction.PaymentPartRecord = Part.Record;
            _transactionRepository.Create(Transaction);
        }

        public void UpdateTransaction(PaymentTransactionRecord Transaction) {
            if (Transaction == null) {
                throw new ArgumentNullException("Transaction", "Transaction cannot be null.");
            }
            _transactionRepository.Update(Transaction);
        }
    }
}