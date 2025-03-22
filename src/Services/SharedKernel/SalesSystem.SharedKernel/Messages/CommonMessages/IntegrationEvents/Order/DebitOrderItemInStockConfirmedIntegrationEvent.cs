namespace SalesSystem.SharedKernel.Messages.CommonMessages.IntegrationEvents.Order
{
    public record DebitOrderItemInStockConfirmedIntegrationEvent(Guid OrderId, Guid CustomerId) : IntegrationEvent;
}
