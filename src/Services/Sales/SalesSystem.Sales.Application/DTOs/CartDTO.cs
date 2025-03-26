namespace SalesSystem.Sales.Application.DTOs
{
    public record CartDTO(
        Guid OrderId,
        decimal SubTotal,
        decimal TotalPrice,
        decimal TotalDiscount,
        string? VoucherCode,
        List<CartItemDTO> Items);
}