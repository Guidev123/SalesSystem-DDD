using MidR.Interfaces;
using SalesSystem.SharedKernel.Data.EventSourcing;
using SalesSystem.SharedKernel.Events;
using SalesSystem.SharedKernel.Events.DomainEvents;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.SharedKernel.Abstractions.Mediator
{
    public sealed class MediatorHandler(IMediator mediator,
                                        IEventSourcingRepository eventSourcingRepository)
                                      : IMediatorHandler
    {
        public async Task PublishEventAsync<T>(T @event) where T : Event
        {
            await mediator.PublishToBusAsync(@event);

            if (!@event.GetType().BaseType!.Name.Equals(nameof(DomainEvent)))
                await eventSourcingRepository.SaveAsync(@event);
        }

        public async Task<Response<T>> SendCommandAsync<T>(Command<T> command) => await mediator.SendAsync(command);

        public async Task<Response<T>> SendQueryAsync<T>(IQuery<T> query) => await mediator.SendAsync(query);

        public async Task<PagedResponse<T>> SendQueryAsync<T>(IPagedQuery<T> query) => await mediator.SendAsync(query);
    }
}