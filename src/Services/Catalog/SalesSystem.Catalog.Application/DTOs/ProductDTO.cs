namespace SalesSystem.Catalog.Application.DTOs
{
    public record ProductDTO(
        string Name, string Description, string Image,
        decimal Price, int QuantityInStock, decimal Height,
        decimal Width, decimal Depth, IEnumerable<CategoryDTO> Categories
        );
}
