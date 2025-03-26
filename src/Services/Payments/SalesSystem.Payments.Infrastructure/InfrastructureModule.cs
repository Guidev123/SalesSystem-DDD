using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Payments.ACL.Interfaces;
using SalesSystem.Payments.ACL.Services;
using SalesSystem.Payments.Application.Services;
using SalesSystem.Payments.Domain.Repositories;
using SalesSystem.Payments.Infrastructure.Persistence;
using SalesSystem.Payments.Infrastructure.Persistence.Respositories;

namespace SalesSystem.Payments.Infrastructure
{
    public static class InfrastructureModule
    {
        public static void AddPaymentsModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices();
            services.AddPaymentDbContext(configuration);
            services.AddRepositories();
        }

        public static void AddPaymentDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PaymentDbContext>(x =>
                    x.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? string.Empty)
                    .LogTo(Console.WriteLine)
                    .EnableDetailedErrors());
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPaymentFacade, PaymentFacade>();
            services.AddScoped<IStripeService, StripeService>();
        }
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPaymentRepository, PaymentRepository>();
        }
    }
}
