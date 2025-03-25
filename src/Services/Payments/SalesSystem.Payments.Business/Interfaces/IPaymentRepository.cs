using SalesSystem.Payments.Business.Models;
using SalesSystem.SharedKernel.Data;

namespace SalesSystem.Payments.Business.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        void Create(Payment payment);
        void CreateTransaction(Transaction transaction);
    }
}
