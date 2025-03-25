namespace SalesSystem.SharedKernel.Communication.DTOs
{
    public record PaymentOrderDTO(
        Guid OrderId,
        Guid CustomerId,
        string CustomerEmail,
        string OrderCode,
        decimal Total
        );
}
