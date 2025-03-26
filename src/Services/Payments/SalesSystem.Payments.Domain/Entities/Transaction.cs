using SalesSystem.Payments.Domain.Enums;
using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Payments.Domain.Entities
{
    public class Transaction : Entity
    {
        public Transaction(Guid orderId, Guid paymentId, decimal total, string orderCode)
        {
            OrderId = orderId;
            PaymentId = paymentId;
            Total = total;
            Status = ETransactionStatus.WaitingPayment;
            OrderCode = orderCode;
            Validate();
        }

        public Guid OrderId { get; private set; }
        public Guid PaymentId { get; private set; }
        public string OrderCode { get; private set; }
        public decimal Total { get; private set; }
        public ETransactionStatus Status { get; private set; }
        public string? ExternalReference { get; private set; }
        public Payment? Payment { get; private set; }

        public void SetAsPaid(string externalReference)
        {
            AssertionConcern.EnsureNotEmpty(externalReference, "External Reference must be not empty.");
            Status = ETransactionStatus.Paid;
            ExternalReference = externalReference;
        }

        public override void Validate()
        {
            AssertionConcern.EnsureDifferent(OrderId, Guid.Empty, "Order id cannot be empty.");
            AssertionConcern.EnsureDifferent(PaymentId, Guid.Empty, "Payment id cannot be empty.");
            AssertionConcern.EnsureGreaterThan(Total, 0, "Total must be greater than 0.");
            AssertionConcern.EnsureNotEmpty(OrderCode, "Order code must be not empty.");
        }
    }
}
