namespace SalesSystem.SharedKernel.DTOs
{
    public record OrderProductsListDto(
        Guid OrderId,
        ICollection<ItemDto> Items
        );
}