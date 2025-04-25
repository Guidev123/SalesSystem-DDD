using SalesSystem.Catalog.Domain.Entities;
using SalesSystem.SharedKernel.Data;

namespace SalesSystem.Catalog.Domain.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<(IEnumerable<Product> pagedProducts, int totalCount)> GetAllAsync(int pageNumber, int pageSize);

        Task<(IEnumerable<Product> pagedProducts, int totalCount)> GetByCategoryAsync(int pageNumber, int pageSize, int code);

        Task<(IEnumerable<Category> pagedCategories, int totalCount)> GetAllCategoriesAsync(int pageNumber, int pageSize);

        Task<Product?> GetByIdAsync(Guid id);

        void Create(Product product);

        void CreateCategory(Category category);

        void Update(Product product);

        void UpdateCategory(Category category);
    }
}