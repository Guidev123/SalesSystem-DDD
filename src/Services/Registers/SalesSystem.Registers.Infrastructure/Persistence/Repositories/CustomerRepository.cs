using Microsoft.EntityFrameworkCore;
using SalesSystem.Registers.Domain.Entities;
using SalesSystem.Registers.Domain.Repositories;
using SalesSystem.SharedKernel.Data;

namespace SalesSystem.Registers.Infrastructure.Persistence.Repositories
{
    public sealed class CustomerRepository(RegistersDbContext context) : ICustomerRepository
    {
        public IUnitOfWork UnitOfWork => context;

        public async Task<bool> AlreadyExists(string document)
            => await context.
            Customers.AsNoTracking()
            .AnyAsync(x => x.Document.Number
            .Equals(document, StringComparison.OrdinalIgnoreCase));

        public void Create(Customer customer)
            => context.Customers.Add(customer);

        public void CreateAddress(Address address)
            => context.Addresses.Add(address);

        public async Task<Customer?> GetByEmailAsync(string email)
            => await context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Address.Equals(email, StringComparison.OrdinalIgnoreCase));

        public async Task<Customer?> GetByIdAsync(Guid id)
            => await context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Customer?> GetCustomerAddressByIdAsync(Guid id)
            => await context.Customers.AsNoTrackingWithIdentityResolution().Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Address?> GetAddressByCustomerIdAsync(Guid customerId)
            => await context.Addresses.AsNoTracking().FirstOrDefaultAsync(x => x.CustomerId == customerId);

        public void Update(Customer customer)
            => context.Customers.Update(customer);

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}