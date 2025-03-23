using SalesSystem.SharedKernel.Communication.DTOs;

namespace SalesSystem.SharedKernel.Messages.CommonMessages.IntegrationEvents.Order
{
    public record StartedOrderIntegrationEvent(
        Guid OrderId,
        Guid CustomerId,
        decimal Total,
        OrderProductsListDTO OrderProductsList
        ) : IntegrationEvent;
}