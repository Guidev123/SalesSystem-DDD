using SalesSystem.Register.Application.DTOs;
using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Register.Application.Queries.Customers.GetAddress
{
    public record GetAddressByCustomerQuery(Guid CustomerId) : IQuery<AddressDTO>;
}