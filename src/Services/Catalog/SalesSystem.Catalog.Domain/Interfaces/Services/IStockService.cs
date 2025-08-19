using SalesSystem.SharedKernel.Models;

namespace SalesSystem.Catalog.Domain.Interfaces.Services
{
    public interface IStockService : IDisposable
    {
        Task<bool> DebitStockAsync(Guid productId, int quantity);

        Task<bool> DebitListStockAsync(OrderProductsList orderProductsList);

        Task<bool> AddStockAsync(Guid productId, int quantity);

        Task<bool> AddListStockAsync(OrderProductsList orderProductsList);
    }
}