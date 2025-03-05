using SalesSystem.Catalog.Application.DTOs;
using SalesSystem.Catalog.Domain.Entities;

namespace SalesSystem.Catalog.Application.Mappers
{
    public static class ProductMapper
    {
        public static CategoryDTO MapFromEntity(this Category category)
            => new(category.Id, category.Name, category.Code);

        public static ProductDTO MapFromEntity(this Product product)
            => new(product.Id, product.Name, product.Description, product.ImageUrl, product.Price, product.QuantityInStock,
                product.Dimensions.Height, product.Dimensions.Width, product.Dimensions.Depth, product.Category.MapFromEntity());
    }
}
