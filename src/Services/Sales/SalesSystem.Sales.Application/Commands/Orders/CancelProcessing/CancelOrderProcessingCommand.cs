using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Sales.Application.Commands.Orders.CancelProcessing
{
    public record CancelOrderProcessingCommand : Command<CancelOrderProcessingResponse>
    {
        public CancelOrderProcessingCommand(Guid orderId, Guid customerId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
    }
}