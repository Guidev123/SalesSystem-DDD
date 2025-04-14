namespace SalesSystem.SharedKernel.DTOs
{
    public record OrderProductsListDTO(
        Guid OrderId,
        ICollection<ItemDTO> Items
        );
}