namespace SalesSystem.SharedKernel.Communication.DTOs
{
    public record OrderProductsListDTO(
        Guid OrderId,
        ICollection<ItemDTO> Items
        );
}
