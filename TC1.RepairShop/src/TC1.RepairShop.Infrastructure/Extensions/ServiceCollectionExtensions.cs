using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TC1.RepairShop.Infrastructure.Data;

namespace TC1.RepairShop.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepairShopDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<RepairShopDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
