using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TC1.RepairShop.Domain.Interfaces.Parts;
using TC1.RepairShop.Domain.Interfaces.Quotes;
using TC1.RepairShop.Domain.Interfaces.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces.Services;
using TC1.RepairShop.Domain.Interfaces.Users;
using TC1.RepairShop.Domain.Interfaces.Vehicles;
using TC1.RepairShop.Infrastructure.Data;
using TC1.RepairShop.Infrastructure.Data.Repositories;

namespace TC1.RepairShop.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString =
            configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<RepairShopDbContext>(options =>
                options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(15),
                    errorNumbersToAdd: null)
                )
            );

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
            services.AddScoped<IServiceOrderPartRepository, ServiceOrderPartRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IPartRepository, PartRepository>();

            return services;
        }
    }
}
