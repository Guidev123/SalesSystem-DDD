using MediatR;
using SalesSystem.Register.Application.Commands.Customers.Create;
using SalesSystem.SharedKernel.Communication.Mediator;

namespace SalesSystem.Register.Application.Events.Handlers
{
    public sealed class UserCreatedEventHandler(IMediatorHandler mediator) : INotificationHandler<UserCreatedEvent>
    {
        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            await mediator.SendCommand(new CreateCustomerCommand(notification.Id, notification.Name,
                notification.Email, notification.Document,
                notification.BirthDate));
        }
    }
}