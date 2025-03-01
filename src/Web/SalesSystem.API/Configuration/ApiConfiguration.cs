using SalesSystem.API.Middlewares;
using SalesSystem.SharedKernel.EventBus;
using System.Reflection;

namespace SalesSystem.API.Configuration
{
    public static class ApiConfiguration
    {
        public static void AddConfigurations(this WebApplicationBuilder builder)
        {
            builder.AddMediatRBus();
            builder.AddHandlers();
            builder.AddCustomMiddlewares();
        }

        public static void AddMediatRBus(this WebApplicationBuilder builder)
            => builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();

        public static void AddHandlers(this WebApplicationBuilder builder)
            => builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies([.. Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "SalesSystem.*.dll").Select(Assembly.LoadFrom)]));

        public static void AddCustomMiddlewares(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<GlobalExceptionMiddleware>();
        }
    }
}
