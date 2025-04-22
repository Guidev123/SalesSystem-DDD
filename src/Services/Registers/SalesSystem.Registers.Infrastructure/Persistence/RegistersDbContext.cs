using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalesSystem.EventSourcing;
using SalesSystem.Registers.Domain.Entities;
using SalesSystem.Registers.Infrastructure.Models;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.Data;
using SalesSystem.SharedKernel.Events;
using System.Reflection;

namespace SalesSystem.Registers.Infrastructure.Persistence
{
    public sealed class RegistersDbContext(DbContextOptions<RegistersDbContext> options,
                                          IMediatorHandler mediatorHandler)
                                        : IdentityDbContext<User>(options), IUnitOfWork
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Ignore<Event>();

            var properties = builder.Model.GetEntityTypes()
                .SelectMany(x => x.GetProperties())
                .Where(x => x.ClrType == typeof(string) && x.GetColumnType() == null);

            builder.HasDefaultSchema("registers");

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var property in properties)
            {
                property.SetColumnType("varchar(160)");
                property.SetIsUnicode(false);
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