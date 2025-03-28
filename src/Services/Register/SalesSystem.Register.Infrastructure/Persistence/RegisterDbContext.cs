using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Register.Domain.Entities;
using SalesSystem.SharedKernel.Data;
using SalesSystem.SharedKernel.Messages;
using System.Reflection;

namespace SalesSystem.Register.Infrastructure.Persistence
{
    public sealed class RegisterDbContext(DbContextOptions<RegisterDbContext> options)
                                        : IdentityDbContext(options), IUnitOfWork
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Ignore<Event>();

            var properties = builder.Model.GetEntityTypes()
                .SelectMany(x => x.GetProperties())
                .Where(x => x.ClrType == typeof(string) && x.GetColumnType() == null);

            builder.HasDefaultSchema("register");

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var property in properties)
            {
                property.SetColumnType("varchar(160)");
                property.SetIsUnicode(false);
            }
        }

        public Task<bool> CommitAsync()
        {
            throw new NotImplementedException();
        }
    }
}
