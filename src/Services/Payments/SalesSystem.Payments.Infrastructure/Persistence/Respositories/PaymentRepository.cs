using Microsoft.EntityFrameworkCore;
using SalesSystem.Payments.Domain.Entities;
using SalesSystem.Payments.Domain.Repositories;
using SalesSystem.SharedKernel.Data;

namespace SalesSystem.Payments.Infrastructure.Persistence.Respositories
{
    public sealed class PaymentRepository(PaymentDbContext context) : IPaymentRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public void Create(Payment payment)
            => context.Payments.Add(payment);

        public void CreateTransaction(Transaction transaction)
            => context.Transactions.Add(transaction);

        public async Task<Payment?> GetByCustomerIdAsync(Guid customerId)
            => await context.Payments.AsNoTracking().FirstOrDefaultAsync(x => x.CustomerId == customerId);

        public void Update(Payment payment)
            => context.Payments.Update(payment);

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
