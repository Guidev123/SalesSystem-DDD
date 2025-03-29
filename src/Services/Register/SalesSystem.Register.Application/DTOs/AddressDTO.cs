namespace SalesSystem.Register.Application.DTOs
{
    public record AddressDTO(
        Guid CustomerId, string Street, string Number,
        string AdditionalInfo, string Neighborhood,
        string ZipCode, string City, string State
        );
}
