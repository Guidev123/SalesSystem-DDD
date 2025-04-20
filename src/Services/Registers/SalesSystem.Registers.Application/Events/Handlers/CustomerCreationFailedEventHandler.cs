using MidR.Interfaces;
using SalesSystem.Registers.Application.Commands.Authentication.Delete;
using SalesSystem.SharedKernel.Abstractions.Mediator;

namespace SalesSystem.Registers.Application.Events.Handlers
{
    public sealed class CustomerCreationFailedEventHandler(IMediatorHandler mediator)
                                                         : INotificationHandler<CustomerCreationFailedEvent>
    {
        public async Task ExecuteAsync(CustomerCreationFailedEvent notification, CancellationToken cancellationToken)
        {
            await mediator.SendCommand(new DeleteUserCommand(notification.Id));
        }
    }
}