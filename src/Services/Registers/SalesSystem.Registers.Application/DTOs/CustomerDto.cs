namespace SalesSystem.Registers.Application.DTOs
{
    public record CustomerDto(
        Guid Id, string Email,
        AddressDto? Address,
        IReadOnlyCollection<string>? Roles
        );
}