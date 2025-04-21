using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Sales.Application.Commands.Orders.RemoveOrderItem
{
    public record RemoveOrderItemCommand : Command<RemoveOrderItemResponse>
    {
        public RemoveOrderItemCommand(Guid productId)
        {
            ProductId = productId;
        }

        public Guid CustomerId { get; private set; }
        public Guid ProductId { get; }

        public void SetCustomerId(Guid customerId)
        {
            CustomerId = customerId;
            AggregateId = customerId;
        }
    }
}