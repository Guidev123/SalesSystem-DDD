using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Sales.Application.Commands.Orders.RemoveOrderItem
{
    public record RemoveOrderItemCommand : Command<RemoveOrderItemResponse>
    {
        public RemoveOrderItemCommand(Guid orderId, Guid productId)
        {
            OrderId = orderId;
            ProductId = productId;
        }

        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; }
        public Guid ProductId { get; }

        public void SetCustomerId(Guid customerId)
        {
            CustomerId = customerId;
            AggregateId = customerId;
        }
    }
}