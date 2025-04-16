using SalesSystem.Registers.Application.DTOs;
using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Registers.Application.Queries.Customers.GetAddress
{
    public record GetAddressByCustomerQuery(Guid CustomerId) : IQuery<AddressDto>;
}