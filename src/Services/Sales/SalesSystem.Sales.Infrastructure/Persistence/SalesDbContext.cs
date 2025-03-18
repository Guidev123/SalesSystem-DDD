using Microsoft.EntityFrameworkCore;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.Sales.Infrastructure.Extensions;
using SalesSystem.SharedKernel.Communication.Mediator;
using SalesSystem.SharedKernel.Data;
using SalesSystem.SharedKernel.Messages;
using System.Reflection;

namespace SalesSystem.Sales.Infrastructure.Persistence
{
    public sealed class SalesDbContext(DbContextOptions<SalesDbContext> options, IMediatorHandler mediatorHandler)
                      : DbContext(options), IUnitOfWork
    {

        public DbSet<Order> Orders { get; set; } 
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            var properties = modelBuilder.Model.GetEntityTypes()
                .SelectMany(x => x.GetProperties())
                .Where(x => x.ClrType == typeof(string) && x.GetColumnType() == null);

            modelBuilder.HasDefaultSchema("sales");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            foreach (var property in properties)
            {
                property.SetColumnType("varchar(160)");
                property.SetIsUnicode(false);
            }
        }

        public async Task<bool> CommitAsync()
        {
            var success = await SaveChangesAsync() > 0;
            if(success) await mediatorHandler.PublishEventsAsync(this);

            return success;
        }
    }
}
