namespace SalesSystem.SharedKernel.Messages
{
    public abstract class Message
    {
        protected Message() => MessageType = GetType().Name;

        public string MessageType { get; }
        public Guid AggregateId { get; protected set; }
    }
}
