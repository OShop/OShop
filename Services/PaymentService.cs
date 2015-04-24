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

        public void AddTransaction(PaymentPart Part, string Method, decimal Amount, string TransactionId = "", TransactionStatus Status = TransactionStatus.Pending, DateTime? Date = null) {
            _transactionRepository.Create(new PaymentTransactionRecord() {
                PaymentPartRecord = Part.Record,
                Method = Method,
                TransactionId = TransactionId,
                Amount = Amount,
                Date = Date.HasValue ? Date.Value : _clock.UtcNow,
                Status = Status
            });
        }


        public void UpdateTransaction(int Id, string Method = null, decimal? Amount = null, string TransactionId = null, TransactionStatus? Status = null, DateTime? Date = null) {
            var transaction = _transactionRepository.Get(Id);
            if (transaction != null) {
                if (Method != null) {
                    transaction.Method = Method;
                }
                if (Amount.HasValue) {
                    transaction.Amount = Amount.Value;
                }
                if (TransactionId != null) {
                    transaction.TransactionId = TransactionId;
                }
                if (Status.HasValue) {
                    transaction.Status = Status.Value;
                }
                if (Date.HasValue) {
                    transaction.Date = Date.Value;
                }
                _transactionRepository.Update(transaction);
            }
        }
    }
}