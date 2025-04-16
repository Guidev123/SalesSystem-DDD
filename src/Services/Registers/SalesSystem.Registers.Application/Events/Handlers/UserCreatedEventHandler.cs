using MidR.Interfaces;
using SalesSystem.Registers.Application.Commands.Customers.Create;
using SalesSystem.SharedKernel.Abstractions.Mediator;

namespace SalesSystem.Registers.Application.Events.Handlers
{
    public sealed class UserCreatedEventHandler(IMediatorHandler mediator) : INotificationHandler<UserCreatedEvent>
    {
        public async Task ExecuteAsync(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            await mediator.SendCommand(new CreateCustomerCommand(notification.Id, notification.Name,
                notification.Email, notification.Document,
                notification.BirthDate));
        }
    }
}