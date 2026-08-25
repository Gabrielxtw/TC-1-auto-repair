using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TC1.RepairShop.Application.Notifications;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Infrastructure.Data;
using TC1.RepairShop.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Options;
using SendGrid;
using TC1.RepairShop.Infrastructure.Email;

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
            services.AddScoped<IServiceOrderServiceRepository, ServiceOrderServiceRepository>();



            // Bind SendGrid options from appsettings
            var apiKey = configuration[$"{SendGridOptions.SectionName}:ApiKey"];
            var fromEmail = configuration[$"{SendGridOptions.SectionName}:FromEmail"];
            var fromName = configuration[$"{SendGridOptions.SectionName}:FromName"];

            services.Configure<SendGridOptions>(opts =>
            {
                opts.ApiKey = apiKey ?? string.Empty;
                opts.FromEmail = fromEmail ?? string.Empty;
                opts.FromName = fromName ?? string.Empty;
            });

            // Register SendGrid client using loaded options
            services.AddSingleton(sp =>
            {
                var options = sp.GetRequiredService<IOptions<SendGridOptions>>().Value;
                return new SendGridClient(options.ApiKey);
            });

            services.AddSingleton<EmailQueue>();
            services.AddSingleton<IEmailSender>(sp => sp.GetRequiredService<EmailQueue>());
            services.AddHostedService<EmailQueueBackgroundService>();

            return services;
        }
    }
}
