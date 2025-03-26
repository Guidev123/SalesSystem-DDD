using SalesSystem.Payments.Business.Models;
using SalesSystem.SharedKernel.Data;

namespace SalesSystem.Payments.Business.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<Payment> GetByCustomerIdAsync(Guid customerId);
        void Create(Payment payment);
        void CreateTransaction(Transaction transaction);
        void Update(Payment payment);
    }
}
