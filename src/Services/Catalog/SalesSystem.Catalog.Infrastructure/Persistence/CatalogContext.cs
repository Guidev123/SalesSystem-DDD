using Microsoft.EntityFrameworkCore;
using SalesSystem.Catalog.Domain.Entities;
using SalesSystem.SharedKernel.Data;
using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Catalog.Infrastructure.Persistence
{
    public class CatalogContext(DbContextOptions<CatalogContext> options) : DbContext(options), IUnitOfWork
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

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogContext).Assembly);

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

            return await base.SaveChangesAsync() > 0;
        }
    }
}
