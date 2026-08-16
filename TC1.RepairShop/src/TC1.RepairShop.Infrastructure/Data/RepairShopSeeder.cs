using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Infrastructure.Data
{
    public static class RepairShopSeeder
    {
        public static async Task SeedAdminAsync(RepairShopDbContext context, IConfiguration configuration)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            const string defaultUsername = "admin";

            var exists = await context.Users.AnyAsync(u => u.Username == defaultUsername);
            if (exists) return;

            var password = configuration["SeedAdmin:Password"]
                ?? Environment.GetEnvironmentVariable("SEED_ADMIN_PASSWORD");

            if (string.IsNullOrWhiteSpace(password))
            {
                // Do not seed if no password provided
                return;
            }

            // Values chosen to satisfy domain validations
            var document = configuration["SeedAdmin:Document"] ?? "01098843371";
            var email = configuration["SeedAdmin:Email"] ?? "admin@example.com";
            var phone = configuration["SeedAdmin:Phone"] ?? "11999999999";

            var admin = User.Create(defaultUsername, password, document, email, UserRole.Admin, phone);

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}
