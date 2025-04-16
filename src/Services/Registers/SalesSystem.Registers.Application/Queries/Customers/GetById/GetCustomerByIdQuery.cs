using SalesSystem.Registers.Application.DTOs;
using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Registers.Application.Queries.Customers.GetById
{
    public record GetCustomerByIdQuery(Guid CustomerId) : IQuery<CustomerDto>
    {
    }
}