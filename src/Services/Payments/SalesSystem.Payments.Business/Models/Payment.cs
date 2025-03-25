using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Payments.Business.Models
{
    public class Payment : Entity, IAggregateRoot
    {
        public Payment(Guid orderId, decimal amount)
        {
            OrderId = orderId;
            Amount = amount;
            Validate();
        }

        public Guid OrderId { get; private set; }
        public string? Status { get; private set; }
        public decimal Amount { get; private set; }
        public Transaction? Transaction { get; private set; }


        public override void Validate()
        {
            AssertionConcern.EnsureGreaterThan(Amount, 0, "Amount must be greater than 0.");
            AssertionConcern.EnsureDifferent(OrderId, Guid.Empty, "Order id cannot be empty.");
        }
    }
}
