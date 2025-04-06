namespace SalesSystem.Register.Application.DTOs
{
    public record CustomerDTO(
        Guid Id, string Email,
        AddressDTO? Address,
        IReadOnlyCollection<string>? Roles
        );
}