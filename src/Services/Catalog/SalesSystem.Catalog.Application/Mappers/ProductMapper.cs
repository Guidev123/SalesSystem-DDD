using SalesSystem.Catalog.Application.Commands.Products.Create;
using SalesSystem.Catalog.Application.DTOs;
using SalesSystem.Catalog.Domain.Entities;

namespace SalesSystem.Catalog.Application.Mappers
{
    public static class ProductMapper
    {
        public static CategoryDto MapFromEntity(this Category category)
            => new(category.Id, category.Name, category.Code);

        public static ProductDto MapFromEntity(this Product product)
            => new(product.Id, product.Name, product.Description, product.ImageUrl, product.Price, product.QuantityInStock,
                product.Dimensions.Height, product.Dimensions.Width, product.Dimensions.Depth, product.Category.MapFromEntity());

        public static Product MapToEntity(this CreateProductCommand command, string imageUrl)
            => new(command.Name, command.Description, command.Price, imageUrl, command.CategoryId, new(command.Height, command.Width, command.Depth));
    }
}