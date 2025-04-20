using MidR.Interfaces;
using SalesSystem.Registers.Application.Commands.Customers.Delete;
using SalesSystem.SharedKernel.Abstractions.Mediator;

namespace SalesSystem.Registers.Application.Events.Handlers
{
    public sealed class UserDeletedEventHandler(IMediatorHandler mediatorHandler) : INotificationHandler<UserDeletedEvent>
    {
        public async Task ExecuteAsync(UserDeletedEvent notification, CancellationToken cancellationToken)
        {
            await mediatorHandler.SendCommand(new DeleteCustomerCommand(notification.UserId));
        }
    }
}
