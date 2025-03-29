using MediatR;
using SalesSystem.Register.Application.Commands.Authentication.Delete;
using SalesSystem.SharedKernel.Communication.Mediator;

namespace SalesSystem.Register.Application.Events.Handlers
{
    public sealed class CustomerCreationFailedEventHandler(IMediatorHandler mediator)
                                                         : INotificationHandler<CustomerCreationFailedEvent>
    {
        public async Task Handle(CustomerCreationFailedEvent notification, CancellationToken cancellationToken)
        {
            await mediator.SendCommand(new DeleteUserCommand(notification.Id, notification.Email));
        }
    }
}