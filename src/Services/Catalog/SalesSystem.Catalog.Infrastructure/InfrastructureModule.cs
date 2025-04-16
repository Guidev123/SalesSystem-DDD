using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Catalog.Application.Storage;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.Catalog.Domain.Interfaces.Services;
using SalesSystem.Catalog.Domain.Services;
using SalesSystem.Catalog.Infrastructure.Persistence;
using SalesSystem.Catalog.Infrastructure.Persistence.Repositories;
using SalesSystem.Catalog.Infrastructure.Storage;

namespace SalesSystem.Catalog.Infrastructure
{
    public static class InfrastructureModule
    {
        public static void AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCatalogDbContext(configuration);
            services.AddRedisCache(configuration);
            services.AddRepositories();
            services.AddDomainServices();
        }

        public static void AddCatalogDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
        }

        public static void AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IStockService, StockService>();
        }

        public static void AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(op =>
            {
                op.InstanceName = configuration["CacheDataSettings:InstanceName"];
                op.Configuration = configuration["CacheDataSettings:Configuration"];
            });

            services.AddTransient<ICacheService, RedisService>();
        }
    }
}