using SalesSystem.Register.Application.DTOs;
using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Register.Application.Queries.Customers.GetById
{
    public record GetCustomerByIdQuery(Guid CustomerId) : IQuery<CustomerDto>
    {
    }
}