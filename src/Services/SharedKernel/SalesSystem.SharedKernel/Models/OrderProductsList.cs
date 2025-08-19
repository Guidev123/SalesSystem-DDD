namespace SalesSystem.SharedKernel.Models
{
    public record OrderProductsList(
        Guid OrderId,
        ICollection<Item> Items
        );
}