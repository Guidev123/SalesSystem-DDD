using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Events;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.SharedKernel.Abstractions.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEventAsync<T>(T @event) where T : Event;

        Task<Response<T>> SendCommand<T>(Command<T> command);

        Task<Response<T>> SendQuery<T>(IQuery<T> query);

        Task<PagedResponse<T>> SendQuery<T>(IPagedQuery<T> query);
    }
}