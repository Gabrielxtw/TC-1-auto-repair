using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.UnitTests.Vehicles;

namespace TC1.RepairShop.IntegrationTests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string AdminUsername = "admin";
    public const string AdminPassword = "Admin@123";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IUserRepository>();
            services.AddScoped<IUserRepository, FakeUserRepository>();

            services.RemoveAll<IVehicleRepository>();
            services.AddScoped<IVehicleRepository, FakeVehicleRepository>();

            services.RemoveAll<IPartRepository>();
            services.AddScoped<IPartRepository, FakePartRepository>();

            services.RemoveAll<IServiceRepository>();
            services.AddScoped<IServiceRepository, FakeServiceRepository>();

            services.RemoveAll<IQuoteRepository>();
            services.AddScoped<IQuoteRepository, FakeQuoteRepository>();

            services.RemoveAll<IServiceOrderRepository>();
            services.AddScoped<IServiceOrderRepository, FakeServiceOrderRepository>();

            services.RemoveAll<IServiceOrderPartRepository>();
            services.AddScoped<IServiceOrderPartRepository, FakeServiceOrderPartRepository>();

            services.RemoveAll<IServiceOrderServiceRepository>();
            services.AddScoped<IServiceOrderServiceRepository, FakeServiceOrderServiceRepository>();

            SeedAdmin();
        });
    }

    private static readonly object SeedLock = new();

    private static void SeedAdmin()
    {
        lock (SeedLock)
        {
            if (FakeUserRepository.Users.Values.Any(u => u.Username == AdminUsername))
                return;

            var admin = User.Create(AdminUsername, AdminPassword, "01098843371", "admin@example.com", UserRole.Admin, "11999999999");
            FakeUserRepository.Users[admin.Id] = admin;
        }
    }
}
