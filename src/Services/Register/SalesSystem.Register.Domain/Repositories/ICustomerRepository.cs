using SalesSystem.Register.Domain.Entities;
using SalesSystem.SharedKernel.Data;

namespace SalesSystem.Register.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer?> GetByIdAsync(Guid id);

        Task<Customer?> GetCustomerAddressByIdAsync(Guid id);

        Task<Customer?> GetByEmailAsync(string email);

        Task<bool> AlreadyExists(string document);

        void Create(Customer customer);

        void Update(Customer customer);

        void CreateAddress(Address address);
    }
}