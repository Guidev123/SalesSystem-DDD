using SalesSystem.Catalog.Domain.Entities;
using SalesSystem.SharedKernel.Data;

namespace SalesSystem.Catalog.Domain.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task<IEnumerable<Product>> GetByCategoryAsync(int code);

        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        Task<Product?> GetByIdAsync(Guid id);

        void Create(Product product);

        void CreateCategory(Category category);

        void Update(Product product);

        void UpdateCategory(Category category);
    }
}