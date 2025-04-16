using SalesSystem.SharedKernel.Events;

namespace SalesSystem.Registers.Application.Events
{
    public record UserCreatedEvent(
        Guid Id, string Name,
        string Email, string Document,
        DateTime BirthDate
        ) : Event;
}