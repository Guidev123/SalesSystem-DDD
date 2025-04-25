namespace SalesSystem.Sales.Application.DTOs
{
    public record CartDto(
        decimal SubTotal,
        decimal TotalPrice,
        decimal TotalDiscount,
        string? VoucherCode,
        List<CartItemDto> Items);
}