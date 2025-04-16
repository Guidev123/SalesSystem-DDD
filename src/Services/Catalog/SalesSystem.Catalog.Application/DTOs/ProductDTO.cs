namespace SalesSystem.Catalog.Application.DTOs
{
    public record ProductDto(
        Guid Id, string Name, string Description, string Image,
        decimal Price, int QuantityInStock, decimal Height,
        decimal Width, decimal Depth, CategoryDto Category
        );
}