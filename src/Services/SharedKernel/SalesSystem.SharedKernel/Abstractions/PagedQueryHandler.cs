using MidR.Interfaces;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.SharedKernel.Abstractions
{
    public abstract class PagedQueryHandler<TQuery, TResult>(INotificator notificator) : Handler(notificator), IRequestHandler<TQuery, PagedResponse<TResult>>
        where TQuery : IRequest<PagedResponse<TResult>>
    {
        public abstract Task<PagedResponse<TResult>> ExecuteAsync(TQuery request, CancellationToken cancellationToken);
    }
}