using EventStore.Client;

namespace SalesSystem.EventSourcing
{
    public interface IEventStoreService
    {
        EventStoreClient GetStoreClientConnection();
    }
}