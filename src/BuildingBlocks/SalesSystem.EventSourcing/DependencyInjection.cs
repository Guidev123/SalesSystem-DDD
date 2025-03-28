using Microsoft.Extensions.DependencyInjection;
using SalesSystem.SharedKernel.Data.EventSourcing;

namespace SalesSystem.EventSourcing
{
    public static class DependencyInjection
    {
        public static void AddEventStoreConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IEventStoreService, EventStoreService>();
            services.AddSingleton<IEventSourcingRepository, EventSourcingRepository>();
        }
    }
}
