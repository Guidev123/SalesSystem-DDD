using Microsoft.EntityFrameworkCore;
using SalesSystem.EventSourcing;
using SalesSystem.Payments.Domain.Entities;
using SalesSystem.SharedKernel.Communication.Mediator;
using SalesSystem.SharedKernel.Data;
using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Payments.Infrastructure.Persistence
{
    public sealed class PaymentDbContext(DbContextOptions<PaymentDbContext> options,
                                         IMediatorHandler mediatorHandler)
                                       : DbContext(options),
                                         IUnitOfWork
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            modelBuilder.HasDefaultSchema("payments");

            var properties = modelBuilder.Model.GetEntityTypes()
                .SelectMany(p => p.GetProperties())
                .Where(p => p.ClrType == typeof(string)
                && p.GetColumnType() == null);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);

            foreach (var item in properties)
            {
                item.SetColumnType("VARCHAR(160)");
                item.SetIsUnicode(false);
            }
        }

        public async Task<bool> CommitAsync()
        {
            var success = await SaveChangesAsync() > 0;
            if (success) await mediatorHandler.PublishEventsAsync(this);

            return success;
        }
    }
}
