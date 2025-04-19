using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Sales.Application.Commands.Orders.Finish
{
    public record FinishOrderCommand : Command<FinishOrderResponse>
    {
        public FinishOrderCommand(Guid orderId, Guid customerId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
    }
}