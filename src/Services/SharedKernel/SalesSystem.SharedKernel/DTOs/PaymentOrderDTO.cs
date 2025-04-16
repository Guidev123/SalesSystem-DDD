namespace SalesSystem.SharedKernel.DTOs
{
    public record PaymentOrderDto(
        Guid OrderId,
        Guid CustomerId,
        string CustomerEmail,
        string OrderCode,
        decimal Total
        );
}