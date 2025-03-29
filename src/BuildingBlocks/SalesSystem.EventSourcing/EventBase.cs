namespace SalesSystem.EventSourcing
{
    public record EventBase
    {
        public DateTime Timestamp { get; set; }
    }
}