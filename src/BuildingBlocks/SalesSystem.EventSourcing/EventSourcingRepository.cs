using EventStore.Client;
using SalesSystem.SharedKernel.Data.EventSourcing;
using SalesSystem.SharedKernel.Messages;
using System.Text;
using System.Text.Json;

namespace SalesSystem.EventSourcing
{
    public sealed class EventSourcingRepository(IEventStoreService eventStoreService) : IEventSourcingRepository
    {
        public async Task<IEnumerable<StoredEvent>> GetAllAsync(Guid aggregateId)
        {
            var events = eventStoreService.GetStoreClientConnection()
                .ReadStreamAsync(
                    Direction.Forwards,
                    aggregateId.ToString(),
                    StreamPosition.Start,
                    500,
                    resolveLinkTos: false
                );

            List<StoredEvent> resolvedEvents = [];
            await foreach (var resolvedEvent in events)
            {
                var dataEncoded = Encoding.UTF8.GetString(resolvedEvent.Event.Data.Span);
                var jsonData = JsonSerializer.Deserialize<EventBase>(dataEncoded) ?? new();

                var storedEvent = new StoredEvent(
                    resolvedEvent.Event.EventId.ToGuid(),
                    resolvedEvent.Event.EventType,
                    jsonData.Timestamp, dataEncoded);

                resolvedEvents.Add(storedEvent);
            }

            return resolvedEvents.OrderBy(x => x.OccuredAt);
        }

        public async Task SaveAsync<TEvent>(TEvent @event) where TEvent : Event =>
            await eventStoreService.GetStoreClientConnection().AppendToStreamAsync(@event.AggregateId.ToString(),
                WrongExpectedVersion.ExpectedAnyFieldNumber, CreateEventData(@event));

        private static IEnumerable<EventData> CreateEventData<TEvent>(TEvent @event) where TEvent : Event
        {
            yield return new(
                Uuid.NewUuid(),
                @event.MessageType,
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event)),
                null,
                "application/json"
                );
        }
    }
}
