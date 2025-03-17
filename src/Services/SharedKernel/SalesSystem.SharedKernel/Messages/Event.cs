using MediatR;

namespace SalesSystem.SharedKernel.Messages
{
    public abstract record Event : Message, INotification
    {
        protected Event() => OccurredAt = DateTime.Now;
        public DateTime OccurredAt { get; }
    }
}
