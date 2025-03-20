using Microsoft.EntityFrameworkCore;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Data;

namespace SalesSystem.Sales.Infrastructure.Persistence.Repositories
{
    public sealed class OrderRepository(SalesDbContext context) : IOrderRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public void AddOrderItem(OrderItem item)
            => context.OrderItems.Add(item);

        public void Create(Order order)
            => context.Orders.Add(order);

        public void RemoveItem(OrderItem item)
            => context.OrderItems.Remove(item);

        public async Task<IEnumerable<Order>> GetAllByCutomerIdAsync(int pageSize, int pageNumber, Guid customerId)
            => await context.Orders.AsNoTrackingWithIdentityResolution().Include(x => x.OrderItems).Include(x => x.Voucher)
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize).ToListAsync();

        public async Task<Order?> GetByIdAsync(Guid id)
            => await context.Orders.AsNoTrackingWithIdentityResolution().Include(x => x.OrderItems)
            .Include(x => x.Voucher).FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Order?> GetDraftOrderByCustomerIdAsync(Guid orderId)
            => await context.Orders.AsNoTrackingWithIdentityResolution().Include(x => x.OrderItems)
            .Include(x => x.Voucher).FirstOrDefaultAsync(x => x.Id == orderId);

        public async Task<OrderItem?> GetItemByIdAsync(Guid id)
            => await context.OrderItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<OrderItem?> GetItemByOrderIdAsync(Guid orderId, Guid itemId)
            => await context.OrderItems.AsNoTracking().Where(x => x.OrderId == orderId).FirstOrDefaultAsync(x => x.Id == itemId);

        public async Task<Voucher?> GetVoucherByCodeAsync(string code)
            => await context.Vouchers.AsNoTracking().FirstOrDefaultAsync(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        public void Update(Order order)
            => context.Orders.Update(order);

        public void UpdateItem(OrderItem item)
            => context.OrderItems.Update(item);

        public void UpdateVoucher(Voucher voucher)
            => context.Vouchers.Update(voucher);

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}