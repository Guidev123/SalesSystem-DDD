using SalesSystem.API.Middlewares;
using SalesSystem.Payments.ACL.Configurations;
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
            builder.Services.AddJwtConfiguration(builder.Configuration);
            builder.AddHandlers();
            builder.AddCustomMiddlewares();
            builder.AddNotifications();
            builder.Services.AddSwaggerConfig();
            builder.Services.AddHttpContextAccessor();
        }

        public static void AddConfigureMediator(this WebApplicationBuilder builder)
            => builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();

        public static void AddHandlers(this WebApplicationBuilder builder)
            => builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies([.. Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "SalesSystem.*.dll").Select(Assembly.LoadFrom)]));

        public static void AddCustomMiddlewares(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<GlobalExceptionMiddleware>();
        }

        public static void AddModelConfig(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection(nameof(StripeSettings)));
        }

        public static void AddNotifications(this WebApplicationBuilder builder)
            => builder.Services.AddScoped<INotificator, Notificator>();

        public static void AddApiUsing(this WebApplication app)
        {
            app.UseSwaggerConfig();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllers();
        }
    }
}