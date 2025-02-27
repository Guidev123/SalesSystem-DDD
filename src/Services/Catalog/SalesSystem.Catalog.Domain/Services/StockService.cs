using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.Catalog.Domain.Interfaces.Services;

namespace SalesSystem.Catalog.Domain.Services
{
    public sealed class StockService(IProductRepository productRepository) : IStockService
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<bool> AddStockAsync(Guid productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null) return false;

            product.AddStock(quantity);
            _productRepository.Update(product);

            return await _productRepository.UnitOfWork.CommitAsync();
        }

        public async Task<bool> DebitStockAsync(Guid productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if(product is null) return false;

            if(!product.HasStock(quantity)) return false;

            product.DebitStock(quantity);
            _productRepository.Update(product);

            return await _productRepository.UnitOfWork.CommitAsync();
        }

        public void Dispose()
        {
            _productRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
