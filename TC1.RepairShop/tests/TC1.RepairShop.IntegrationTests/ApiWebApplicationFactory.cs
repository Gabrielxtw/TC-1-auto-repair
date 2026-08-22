using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.UnitTests.Vehicles;

namespace TC1.RepairShop.IntegrationTests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IUserRepository>();
            services.AddScoped<IUserRepository, FakeUserRepository>();

            services.RemoveAll<IVehicleRepository>();
            services.AddScoped<IVehicleRepository, FakeVehicleRepository>();
        });
    }
}
