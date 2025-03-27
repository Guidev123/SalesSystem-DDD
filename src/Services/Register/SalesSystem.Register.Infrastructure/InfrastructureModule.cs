using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Register.Infrastructure.Models;
using SalesSystem.Register.Infrastructure.Persistence;

namespace SalesSystem.Register.Infrastructure
{
    public static class InfrastructureModule
    {
        public static void AddRegisterModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRegisterDbContext(configuration);
            services.AddIdentityConfig();
        }

        public static void AddRegisterDbContext(this IServiceCollection services, IConfiguration configuration)
            => services.AddDbContext<RegisterDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        public static void AddIdentityConfig(this IServiceCollection services)
        {
            services.AddDefaultIdentity<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<RegisterDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2);
            });
        }
    }
}
