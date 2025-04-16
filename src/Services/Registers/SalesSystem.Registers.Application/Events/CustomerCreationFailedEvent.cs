using SalesSystem.SharedKernel.Events;

namespace SalesSystem.Registers.Application.Events
{
    public record CustomerCreationFailedEvent(Guid Id, string Email) : Event;
}