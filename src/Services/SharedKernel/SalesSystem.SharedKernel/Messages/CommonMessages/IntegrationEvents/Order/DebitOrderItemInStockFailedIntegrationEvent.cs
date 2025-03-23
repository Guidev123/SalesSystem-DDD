namespace SalesSystem.SharedKernel.Messages.CommonMessages.IntegrationEvents.Order
{
    public record DebitOrderItemInStockFailedIntegrationEvent(Guid OrderId) : IntegrationEvent;
}