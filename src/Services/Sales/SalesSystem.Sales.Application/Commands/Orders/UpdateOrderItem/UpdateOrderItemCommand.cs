using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Sales.Application.Commands.Orders.UpdateOrderItem
{
    public record UpdateOrderItemCommand : Command<UpdateOrderItemResponse>
    {
        public UpdateOrderItemCommand(Guid productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public Guid CustomerId { get; private set; }
        public Guid ProductId { get; }
        public int Quantity { get; }

        public void SetCustomerId(Guid customerId)
            => CustomerId = customerId;

        public void SetAggregateId(Guid aggregateId)
            => AggregateId = aggregateId;
    }
}