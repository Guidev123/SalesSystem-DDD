namespace SalesSystem.Sales.Application.DTOs
{
    public record CartItemDTO(
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice);
}