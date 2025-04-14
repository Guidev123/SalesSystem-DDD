using MediatR;
using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.SharedKernel.Events
{
    public abstract record Event : Message, INotification
    {
        protected Event() => OccurredAt = DateTime.Now;
        public DateTime OccurredAt { get; }
    }
}