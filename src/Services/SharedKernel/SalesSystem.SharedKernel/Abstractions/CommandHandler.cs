using MidR.Interfaces;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.SharedKernel.Abstractions
{
    public abstract class CommandHandler<TCommand, TResult>(INotificator notificator) : Handler(notificator), IRequestHandler<TCommand, Response<TResult>>
                          where TCommand : IRequest<Response<TResult>>

    {
        public abstract Task<Response<TResult>> ExecuteAsync(TCommand request, CancellationToken cancellationToken);
    }
}