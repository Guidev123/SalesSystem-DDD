using SalesSystem.Sales.Domain.Entities;
using SalesSystem.SharedKernel.Data;

namespace SalesSystem.Sales.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetAllByCutomerIdAsync(int pageSize, int pageNumber, Guid customerId);
        Task<Order?> GetDraftOrderByCustomerIdAsync(Guid orderId);
        void Create(Order order);
        void Update(Order order);
        Task<OrderItem?> GetItemByIdAsync(Guid id);
        Task<OrderItem?> GetItemByOrderIdAsync(Guid orderId, Guid itemId);
        void AddOrderItem(OrderItem item);
        void UpdateItem(OrderItem item);    
        void RemoveItem(OrderItem item);
        Task<Voucher?> GetVoucherByCodeAsync(string code);
        void UpdateVoucher(Voucher voucher);    
    }
}
