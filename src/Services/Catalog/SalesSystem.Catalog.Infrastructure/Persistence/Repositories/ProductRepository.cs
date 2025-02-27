using Microsoft.EntityFrameworkCore;
using SalesSystem.Catalog.Domain.Entities;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Data;

namespace SalesSystem.Catalog.Infrastructure.Persistence.Repositories
{
    public sealed class ProductRepository(CatalogContext context) : IProductRepository
    {
        private readonly CatalogContext _context = context;

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Product>> GetAllAsync(int pageNumber, int pageSize)
            => await _context.Products.AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync() => await _context.Categories.AsNoTracking().ToListAsync();

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int pageNumber, int pageSize, int code)
            => await _context.Products.AsNoTrackingWithIdentityResolution().Include(x => x.Category)
            .Where(x => x.Category.Code == code).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        public async Task<Product?> GetByIdAsync(Guid id) => await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        public void Create(Product product) => _context.Products.Add(product);

        public void CreateCategory(Category category) => _context.Categories.Add(category);

        public void Update(Product product) => _context.Products.Update(product);

        public void UpdateCategory(Category category) => _context.Categories.Update(category);
        
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
