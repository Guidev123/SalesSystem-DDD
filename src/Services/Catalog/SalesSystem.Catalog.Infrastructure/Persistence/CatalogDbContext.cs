using Microsoft.EntityFrameworkCore;
using SalesSystem.Catalog.Domain.Entities;
using SalesSystem.EventSourcing;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.Data;
using SalesSystem.SharedKernel.Events;

namespace SalesSystem.Catalog.Infrastructure.Persistence
{
    public sealed class CatalogDbContext(DbContextOptions<CatalogDbContext> options,
                                IMediatorHandler mediatorHandler)
                              : DbContext(options), IUnitOfWork
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            modelBuilder.HasDefaultSchema("catalog");

            var properties = modelBuilder.Model.GetEntityTypes()
                .SelectMany(p => p.GetProperties())
                .Where(p => p.ClrType == typeof(string)
                && p.GetColumnType() == null);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);

            foreach (var item in properties)
            {
                item.SetColumnType("VARCHAR(160)");
                item.SetIsUnicode(false);
            }
        }

        public async Task<bool> CommitAsync()
        {
            foreach (var entry in ChangeTracker.Entries().Where(x => x.Entity.GetType().GetProperty("CreatedAt") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("CreatedAt").CurrentValue = DateTime.Now;

                if (entry.State == EntityState.Modified)
                    entry.Property("CreatedAt").IsModified = false;
            }

            var success = await SaveChangesAsync() > 0;
            if (success) await mediatorHandler.PublishEventsAsync(this);

            return success;
        }
    }
}