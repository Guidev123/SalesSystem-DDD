namespace SalesSystem.Registers.Application.DTOs
{
    public record AddressDto(
        Guid CustomerId, string Street, string Number,
        string AdditionalInfo, string Neighborhood,
        string ZipCode, string City, string State
        );
}