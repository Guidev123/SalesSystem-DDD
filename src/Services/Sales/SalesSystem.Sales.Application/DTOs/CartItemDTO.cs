namespace SalesSystem.Sales.Application.DTOs
{
    public record CartItemDto(
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice);
}