using SalesSystem.API.Middlewares;

namespace SalesSystem.API.Configuration
{
    public static class ApiConfiguration
    {
        public static void AddConfigurations(this WebApplicationBuilder builder)
        {
            builder.AddCustomMiddlewares();
        }

        public static void AddCustomMiddlewares(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<GlobalExceptionMiddleware>();
        }
    }
}
