using SalesSystem.SharedKernel.Events;

namespace SalesSystem.Registers.Application.Events
{
    public record UserDeletedEvent(Guid UserId) : Event;
}
