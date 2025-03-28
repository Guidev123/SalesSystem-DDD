using Microsoft.EntityFrameworkCore;
using SalesSystem.SharedKernel.Communication.Mediator;
using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.EventSourcing
{
    public static class MediatorExtension
    {
        public static async Task PublishEventsAsync(this IMediatorHandler mediatorHandler, DbContext context)
        {
            var domainEntities = context.ChangeTracker.Entries<Entity>()
                .Where(x => x.Entity.Events != null && x.Entity.Events.Count != 0);

            var domainEvents = domainEntities.SelectMany(x => x.Entity.Events).ToList();

            domainEntities.ToList().ForEach(e => e.Entity.PurgeEvents());

            var tasks = domainEvents.Select(async (domainEvents) =>
            {
                await mediatorHandler.PublishEventAsync(domainEvents);
            });

            await Task.WhenAll(tasks);
        }
    }
}