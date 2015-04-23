using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Services;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Services {
    public class PaymentService : IPaymentService {
        private readonly IRepository<PaymentTransactionRecord> _transactionRepository;
        private readonly IClock _clock;

        public PaymentService(
            IRepository<PaymentTransactionRecord> transactionRepository,
            IClock clock) {
            _transactionRepository = transactionRepository;
            _clock = clock;
        }

        public void AddTransaction(PaymentPart part, string Method, decimal Amount, string TransactionId = "", DateTime? Date = null) {
            _transactionRepository.Create(new PaymentTransactionRecord() {
                PaymentPartRecord = part.Record,
                Method = Method,
                TransactionId = TransactionId,
                Amount = Amount,
                Date = Date.HasValue ? Date.Value : _clock.UtcNow
            });
        }
    }
}