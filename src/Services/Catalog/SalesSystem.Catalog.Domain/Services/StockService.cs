using SalesSystem.Catalog.Domain.Events.ProductLowQuantityInStock;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.Catalog.Domain.Interfaces.Services;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.DTOs;

namespace SalesSystem.Catalog.Domain.Services
{
    public sealed class StockService(IProductRepository productRepository, IMediatorHandler mediatorHandler) : IStockService
    {
        private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<bool> AddStockAsync(Guid productId, int quantity)
        {
            if (!await AddStockItemAsync(productId, quantity)) return false;

            return await _productRepository.UnitOfWork.CommitAsync();
        }

        public async Task<bool> AddListStockAsync(OrderProductsListDTO orderProductsList)
        {
            foreach (var orderProduct in orderProductsList.Items)
            {
                if (!await AddStockItemAsync(orderProduct.Id, orderProduct.Quantity)) return false;
            }

            return await _productRepository.UnitOfWork.CommitAsync();
        }

        public async Task<bool> DebitStockAsync(Guid productId, int quantity)
        {
            if (!await DebitStockItemAsync(productId, quantity)) return false;

            return await _productRepository.UnitOfWork.CommitAsync();
        }

        public async Task<bool> DebitListStockAsync(OrderProductsListDTO orderProductsList)
        {
            foreach (var orderProduct in orderProductsList.Items)
            {
                if (!await DebitStockItemAsync(orderProduct.Id, orderProduct.Quantity)) return false;
            }

            return await _productRepository.UnitOfWork.CommitAsync();
        }

        private async Task<bool> DebitStockItemAsync(Guid productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null) return false;

            if (!product.HasStock(quantity)) return false;

            product.DebitStock(quantity);

            if (product.QuantityInStock < 10)
                await _mediatorHandler.PublishEventAsync(new ProductLowQuantityInStockEvent(product.QuantityInStock, product.Id));

            _productRepository.Update(product);
            return true;
        }

        private async Task<bool> AddStockItemAsync(Guid productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null) return false;

            product.AddStock(quantity);

            _productRepository.Update(product);
            return true;
        }

        public void Dispose()
        {
            _productRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}