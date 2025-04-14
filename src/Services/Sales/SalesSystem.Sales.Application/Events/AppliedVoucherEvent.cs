using SalesSystem.SharedKernel.Events;

namespace SalesSystem.Sales.Application.Events
{
    public record AppliedVoucherEvent : Event
    {
        public AppliedVoucherEvent(Guid customerId, Guid orderId, Guid voucherId)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            VoucherId = voucherId;
        }

        public Guid CustomerId { get; }
        public Guid OrderId { get; }
        public Guid VoucherId { get; }
    }
}