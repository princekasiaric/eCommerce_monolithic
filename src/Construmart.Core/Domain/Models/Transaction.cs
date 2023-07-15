using Ardalis.GuardClauses;
using Construmart.Core.DataContracts;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Models
{
    public class Transaction : AuditableModelBase, IAggregateRoot
    {
        public string OrderNumber { get; private set; }
        public string PaymentReference { get; private set; }
        public TransactionStatus TransactionStatus { get; private set; }
        public string JsonResponse { get; private set; }

        private Transaction()
        {
        }

        private Transaction(string orderNumber, string paymentReference, TransactionStatus transactionStatus, string jsonResponse)
        {
            OrderNumber = orderNumber;
            PaymentReference = paymentReference;
            TransactionStatus = transactionStatus;
            JsonResponse = jsonResponse;
        }

        public static Transaction Create(string orderNmuber, string paymentReference, TransactionStatus transactionStatus, string jsonResponse)
        {
            Guard.Against.NullOrWhiteSpace(orderNmuber, nameof(orderNmuber));
            Guard.Against.NullOrWhiteSpace(paymentReference, nameof(paymentReference));
            Guard.Against.Null(transactionStatus, nameof(transactionStatus));
            Guard.Against.NullOrWhiteSpace(jsonResponse, nameof(jsonResponse));
            return new Transaction(orderNmuber, paymentReference, transactionStatus, jsonResponse);
        }
    }
}
