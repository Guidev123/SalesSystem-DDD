using SalesSystem.Payments.Business.Enums;
using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Payments.Business.Models
{
    public class Transaction : Entity
    {
        public Transaction(Guid orderId, Guid paymentId, decimal total)
        {
            OrderId = orderId;
            PaymentId = paymentId;
            Total = total;
            Validate();
        }

        public Guid OrderId { get; private set; }
        public Guid PaymentId { get; private set; }
        public decimal Total { get; private set; }
        public ETransactionStatus Status { get; private set; }
        public string? ExternalReference { get; private set; }

        public override void Validate()
        {
            AssertionConcern.EnsureDifferent(OrderId, Guid.Empty, "Order id cannot be empty.");
            AssertionConcern.EnsureDifferent(PaymentId, Guid.Empty, "Payment id cannot be empty.");
            AssertionConcern.EnsureGreaterThan(Total, 0, "Total must be greater than 0.");
        }
    }
}
