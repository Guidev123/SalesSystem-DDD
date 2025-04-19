using MidR.Interfaces;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.SharedKernel.Abstractions
{
    public abstract class QueryHandler<TQuery, TResult>(INotificator notificator) : Handler(notificator), IRequestHandler<TQuery, Response<TResult>>
        where TQuery : IRequest<Response<TResult>>
    {
        public abstract Task<Response<TResult>> ExecuteAsync(TQuery request, CancellationToken cancellationToken);
    }
}