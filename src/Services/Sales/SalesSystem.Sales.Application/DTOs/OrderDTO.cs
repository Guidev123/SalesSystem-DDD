namespace SalesSystem.Sales.Application.DTOs
{
    public record OrderDTO(
        string Code,
        decimal Price,
        DateTime CreatedAt,
        string Status);
}