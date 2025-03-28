using SalesSystem.Register.Application.DTOs;
using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Register.Application.Queries.Customers.GetById
{
    public record GetCustomerByIdQuery : IQuery<CustomerDTO>
    {

    }
}
