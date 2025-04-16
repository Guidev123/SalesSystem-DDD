using SalesSystem.Registers.Application.Commands.Customers.AddAddress;
using SalesSystem.Registers.Application.Commands.Customers.Create;
using SalesSystem.Registers.Application.DTOs;
using SalesSystem.Registers.Domain.Entities;

namespace SalesSystem.Registers.Application.Mappers
{
    public static class CustomerMappers
    {
        public static Address MapToAddress(this AddAddressCommand command)
            => new(command.CustomerId, command.Street, command.Number,
                command.AdditionalInfo, command.Neighborhood,
                command.ZipCode, command.City, command.State);

        public static Customer MapToCustomer(this CreateCustomerCommand command)
            => new(command.Id, command.Name, command.Email, command.Document, command.BirthDate);

        public static AddressDto MapFromAddress(this Address address)
            => new(address.CustomerId, address.Street, address.Number,
                address.AdditionalInfo, address.Neighborhood,
                address.ZipCode, address.City, address.State);

        public static CustomerDto MapFromCustomer(this Customer customer, IReadOnlyCollection<string>? roles)
        {
            if(customer.Address is not null)
                return new(customer.Id, customer.Email.Address, customer.Address.MapFromAddress(), roles);

            return new(customer.Id, customer.Email.Address, null, roles);
        }
    }
}