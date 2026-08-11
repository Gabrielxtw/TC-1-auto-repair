using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TC1.RepairShop.Application.Clients;
using TC1.RepairShop.Application.Registration;

namespace TC1.RepairShop.IntegrationTests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IUserRepository>();
            services.AddScoped<IUserRepository, FakeUserRepository>();

            services.RemoveAll<ICustomerRepository>();
            services.AddScoped<ICustomerRepository, FakeCustomerRepository>();

            services.RemoveAll<IVehicleRepository>();
            services.AddScoped<IVehicleRepository, FakeVehicleRepository>();
        });
    }
}
