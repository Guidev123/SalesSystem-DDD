namespace SalesSystem.Sales.Application.DTOs
{
    public record CartDTO(
        Guid OrderId,
        Guid CustomerId,
        decimal SubTotal,
        decimal TotalPrice,
        decimal TotalDiscount,
        string? VoucherCode,
        List<CartItemDTO> Items);
}
