using SalesSystem.Payments.Domain.Entities;
using SalesSystem.SharedKernel.Data;

namespace SalesSystem.Payments.Domain.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<Payment?> GetByCustomerIdAsync(Guid customerId);
        void Create(Payment payment);
        void CreateTransaction(Transaction transaction);
        void Update(Payment payment);
    }
}
