using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Register.Application.Events
{
    public record CustomerCreationFailedEvent(Guid Id, string Email) : Event;
}
