using Google.Rpc;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Catalog.Domain.Entities;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Data;

namespace SalesSystem.Catalog.Infrastructure.Persistence.Repositories
{
    public sealed class ProductRepository(CatalogDbContext context) : IProductRepository
    {
        private readonly CatalogDbContext _context = context;

        public IUnitOfWork UnitOfWork => _context;

        public async Task<(IEnumerable<Product> pagedProducts, int totalCount)> GetAllAsync(int pageNumber, int pageSize)
        => (await _context.Products.AsNoTrackingWithIdentityResolution()
            .Include(x => x.Category).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync(),
            await _context.Products.AsNoTracking().CountAsync());

        public async Task<(IEnumerable<Category> pagedCategories, int totalCount)> GetAllCategoriesAsync(int pageNumber, int pageSize)
            => (await _context.Categories.AsNoTracking().Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync(), await _context.Categories.AsNoTracking().CountAsync());

        public async Task<(IEnumerable<Product> pagedProducts, int totalCount)> GetByCategoryAsync(int pageNumber, int pageSize, int code)
            => (await _context.Products.AsNoTrackingWithIdentityResolution().Include(x => x.Category)
            .Where(x => x.Category.Code == code).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync(), await _context.Products.AsNoTracking().Where(x => x.Category.Code == code).CountAsync());

        public async Task<Product?> GetByIdAsync(Guid id)
            => await _context.Products.AsNoTrackingWithIdentityResolution()
            .Include(x => x.Category).FirstOrDefaultAsync(p => p.Id == id);

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