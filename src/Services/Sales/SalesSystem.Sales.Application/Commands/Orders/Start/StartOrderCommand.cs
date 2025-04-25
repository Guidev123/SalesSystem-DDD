using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Sales.Application.Commands.Orders.Start
{
    public record StartOrderCommand : Command<StartOrderResponse>
    {
        public StartOrderCommand(Guid customerId, decimal totalPrice)
        {
            CustomerId = customerId;
            TotalPrice = totalPrice;
        }

        public Guid CustomerId { get; }
        public decimal TotalPrice { get; }

        public void SetAggregateId(Guid aggregateId)
            => AggregateId = aggregateId;
    }
}