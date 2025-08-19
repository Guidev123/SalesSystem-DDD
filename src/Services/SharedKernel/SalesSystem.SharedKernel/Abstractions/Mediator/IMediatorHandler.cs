using SalesSystem.SharedKernel.Events;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.SharedKernel.Abstractions.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEventAsync<T>(T @event) where T : Event;

        Task<Response<T>> SendCommandAsync<T>(Command<T> command);

        Task<Response<T>> SendQueryAsync<T>(IQuery<T> query);

        Task<PagedResponse<T>> SendQueryAsync<T>(IPagedQuery<T> query);
    }
}