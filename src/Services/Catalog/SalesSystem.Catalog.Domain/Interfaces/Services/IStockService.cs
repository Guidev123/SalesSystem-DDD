using SalesSystem.SharedKernel.Communication.DTOs;

namespace SalesSystem.Catalog.Domain.Interfaces.Services
{
    public interface IStockService : IDisposable
    {
        Task<bool> DebitStockAsync(Guid productId, int quantity);
        Task<bool> DebitListStockAsync(OrderProductsListDTO orderProductsList);
        Task<bool> AddStockAsync(Guid productId, int quantity);
        Task<bool> AddListStockAsync(OrderProductsListDTO orderProductsList);
    }
}