using MediatR;
using SalesSystem.SharedKernel.Data.EventSourcing;
using SalesSystem.SharedKernel.Messages;
using SalesSystem.SharedKernel.Messages.CommonMessages.DomainEvents;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.SharedKernel.Communication.Mediator
{
    public sealed class MediatorHandler(IMediator mediator,
                                        IEventSourcingRepository eventSourcingRepository)
                                      : IMediatorHandler
    {
        public async Task PublishEventAsync<T>(T @event) where T : Event
        {
            await mediator.Publish(@event);

            if (!@event.GetType().BaseType!.Name.Equals(nameof(DomainEvent)))
                await eventSourcingRepository.SaveAsync(@event);
        }

        public async Task<Response<T>> SendCommand<T>(Command<T> command) => await mediator.Send(command);

        public async Task<Response<T>> SendQuery<T>(IQuery<T> query) => await mediator.Send(query);

        public async Task<PagedResponse<T>> SendQuery<T>(IPagedQuery<T> query) => await mediator.Send(query);
    }
}