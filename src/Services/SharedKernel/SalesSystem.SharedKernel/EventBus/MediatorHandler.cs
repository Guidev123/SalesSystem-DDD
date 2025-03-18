using MediatR;
using SalesSystem.SharedKernel.Messages;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.SharedKernel.EventBus
{
    public sealed class MediatorHandler(IMediator mediator) : IMediatorHandler
    {
        public async Task PublishEventAsync<T>(T @event) where T : Event => await mediator.Publish(@event);
        public async Task<Response<T>> SendCommand<T>(Command<T> command) => await mediator.Send(command);
        public async Task<Response<T>> SendQuery<T>(IQuery<T> query) => await mediator.Send(query);
        public async Task<PagedResponse<T>> SendQuery<T>(IPagedQuery<T> query) => await mediator.Send(query);
    }
}
