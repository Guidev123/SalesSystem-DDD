namespace SalesSystem.SharedKernel.Models
{
    public record PaymentOrder(
        Guid OrderId,
        Guid CustomerId,
        string CustomerEmail,
        string OrderCode,
        decimal Total
        );
}