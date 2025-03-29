using MediatR;
using SalesSystem.Register.Application.Commands.Customers.AddAddress;
using SalesSystem.Register.Application.Commands.Customers.Create;
using SalesSystem.Register.Application.DTOs;
using SalesSystem.Register.Domain.Entities;

namespace SalesSystem.Register.Application.Mappers
{
    public static class CustomerMappers
    {
        public static Address MapToAddress(this AddAddressCommand command)
            => new(command.CustomerId, command.Street, command.Number,
                command.AdditionalInfo, command.Neighborhood,
                command.ZipCode, command.City, command.State);

        public static Customer MapToCustomer(this CreateCustomerCommand command)
            => new(command.Id, command.Name, command.Email, command.Document, command.BirthDate);

        public static AddressDTO MapFromAddress(this Address address)
            => new(address.CustomerId, address.Street, address.Number,
                address.AdditionalInfo, address.Neighborhood,
                address.ZipCode, address.City, address.State);
    }
}
