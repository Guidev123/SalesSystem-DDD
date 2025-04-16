namespace SalesSystem.Sales.Application.DTOs
{
    public record CartDto(
        Guid OrderId,
        decimal SubTotal,
        decimal TotalPrice,
        decimal TotalDiscount,
        string? VoucherCode,
        List<CartItemDto> Items);
}