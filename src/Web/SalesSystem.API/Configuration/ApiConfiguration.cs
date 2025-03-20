using SalesSystem.API.Middlewares;
using SalesSystem.SharedKernel.Communication.Mediator;
using SalesSystem.SharedKernel.Notifications;
using System.Reflection;

namespace SalesSystem.API.Configuration
{
    public static class ApiConfiguration
    {
        public const int DEFAULT_PAGE_NUMBER = 1;
        public const int DEFAULT_PAGE_SIZE = 15;

        public static void AddConfigurations(this WebApplicationBuilder builder)
        {
            builder.AddConfigureMediator();
            builder.AddHandlers();
            builder.AddCustomMiddlewares();
            builder.AddNotifications();
            builder.Services.AddSwaggerConfig();
        }

        public static void AddConfigureMediator(this WebApplicationBuilder builder)
            => builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();

        public static void AddHandlers(this WebApplicationBuilder builder)
            => builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies([.. Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "SalesSystem.*.dll").Select(Assembly.LoadFrom)]));

        public static void AddCustomMiddlewares(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<GlobalExceptionMiddleware>();
        }

        public static void AddNotifications(this WebApplicationBuilder builder)
            => builder.Services.AddScoped<INotificator, Notificator>();
    }
}