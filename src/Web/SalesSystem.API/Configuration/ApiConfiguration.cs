using SalesSystem.API.Middlewares;
using SalesSystem.Catalog.Infrastructure;

namespace SalesSystem.API.Configuration
{
    public static class ApiConfiguration
    {
        public static void AddConfigurations(this WebApplicationBuilder builder)
        {
            builder.Services.AddCatalogModule(builder.Configuration);
            builder.AddCustomMiddlewares();
        }

        public static void AddCustomMiddlewares(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<GlobalExceptionMiddleware>();
        }
    }
}
