using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.Sales.Infrastructure.Persistence;
using SalesSystem.Sales.Infrastructure.Persistence.Repositories;

namespace SalesSystem.Sales.Infrastructure
{
    public static class InfrastructureModule
    {
        public static void AddSalesModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSalesDbContext(configuration);
            services.AddRepositories();
        }

        public static void AddSalesDbContext(this IServiceCollection services, IConfiguration configuration)
            => services.AddDbContext<SalesDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        public static void AddRepositories(this IServiceCollection services)
            => services.AddScoped<IOrderRepository, OrderRepository>();

    }
}
