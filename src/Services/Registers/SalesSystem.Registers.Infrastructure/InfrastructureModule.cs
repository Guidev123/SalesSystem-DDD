using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Registers.Application.Services;
using SalesSystem.Registers.Domain.Repositories;
using SalesSystem.Registers.Infrastructure.Models;
using SalesSystem.Registers.Infrastructure.Persistence;
using SalesSystem.Registers.Infrastructure.Persistence.Repositories;
using SalesSystem.Registers.Infrastructure.Services;

namespace SalesSystem.Registers.Infrastructure
{
    public static class InfrastructureModule
    {
        public static void AddRegisterModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRegisterDbContext(configuration);
            services.AddRepositories();
            services.AddIdentityConfig();
            services.AddServices();
        }

        public static void AddRegisterDbContext(this IServiceCollection services, IConfiguration configuration)
            => services.AddDbContext<RegistersDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        public static void AddIdentityConfig(this IServiceCollection services)
        {
            services.AddDefaultIdentity<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<RegistersDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2);
            });
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IJwtGeneratorService, JwtGeneratorService>();
        }
    }
}