using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.SharedKernel.EventBus
{
    public interface IMediatorHandler
    {
        Task PublishEventAsync<T>(T @event) where T : Event;
    }
}
