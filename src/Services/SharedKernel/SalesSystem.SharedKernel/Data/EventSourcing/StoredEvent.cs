namespace SalesSystem.SharedKernel.Data.EventSourcing
{
    public record StoredEvent(
        Guid Id, string Type,
        DateTime OccuredAt, string Data
        );
}
