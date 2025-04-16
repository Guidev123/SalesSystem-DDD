namespace SalesSystem.Payments.Application.DTOs
{
    public record ProductDto(
        string Name, int Quantity, decimal Value
        );
}