namespace SalesSystem.Sales.Application.DTOs
{
    public record OrderDto(
        string Code,
        decimal Price,
        DateTime CreatedAt,
        string Status);
}