using MediatR;
using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.SharedKernel.EventBus
{
    public sealed class MediatorHandler(IMediator mediator) : IMediatorHandler
    {
        private readonly IMediator _mediator = mediator;
        public async Task PublishEventAsync<T>(T @event) where T : Event => await _mediator.Publish(@event);
    }
}
