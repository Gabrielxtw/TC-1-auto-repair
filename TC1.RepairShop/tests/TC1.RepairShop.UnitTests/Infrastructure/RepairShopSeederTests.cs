using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using TC1.RepairShop.Infrastructure.Data;
using Xunit;

namespace TC1.RepairShop.UnitTests.Infrastructure;

public class RepairShopSeederTests
{
    private static RepairShopDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<RepairShopDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new RepairShopDbContext(options, Mock.Of<IPublisher>());
    }

    private static IConfiguration CreateConfiguration(Dictionary<string, string?>? values = null)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values ?? new Dictionary<string, string?>())
            .Build();
    }

    [Fact]
    public async Task SeedAdminAsync_WhenNoAdminExistsAndPasswordConfigured_ShouldSeedAdminUser()
    {
        await using var context = CreateContext();
        var configuration = CreateConfiguration(new Dictionary<string, string?>
        {
            ["SeedAdmin:Username"] = "superadmin",
            ["SeedAdmin:Password"] = "Password@123",
            ["SeedAdmin:Document"] = "01098843371",
            ["SeedAdmin:Email"] = "superadmin@example.com",
            ["SeedAdmin:Phone"] = "11999999999",
        });

        await RepairShopSeeder.SeedAdminAsync(context, configuration);

        var created = await context.Users.SingleOrDefaultAsync(u => u.Username == "superadmin");
        Assert.NotNull(created);
        Assert.Equal("superadmin@example.com", created!.Email.Value);
    }

    [Fact]
    public async Task SeedAdminAsync_WhenDefaultUsernameAlreadyExists_ShouldNotSeed()
    {
        await using var context = CreateContext();
        var configuration = CreateConfiguration(new Dictionary<string, string?>
        {
            ["SeedAdmin:Username"] = "admin",
            ["SeedAdmin:Password"] = "Password@123",
        });

        var existing = TC1.RepairShop.Domain.Entities.Users.User.Create(
            "admin", "Existing@123", "01098843371", "existing@example.com",
            TC1.RepairShop.Domain.Enums.UserRole.Admin, "11999999999");
        context.Users.Add(existing);
        await context.SaveChangesAsync();

        await RepairShopSeeder.SeedAdminAsync(context, configuration);

        var count = await context.Users.CountAsync(u => u.Username == "admin");
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task SeedAdminAsync_WhenNoPasswordConfiguredAnywhere_ShouldNotSeed()
    {
        await using var context = CreateContext();
        var configuration = CreateConfiguration(new Dictionary<string, string?>
        {
            ["SeedAdmin:Username"] = "admin",
        });

        var originalEnvValue = Environment.GetEnvironmentVariable("SEED_ADMIN_PASSWORD");
        Environment.SetEnvironmentVariable("SEED_ADMIN_PASSWORD", null);
        try
        {
            await RepairShopSeeder.SeedAdminAsync(context, configuration);
        }
        finally
        {
            Environment.SetEnvironmentVariable("SEED_ADMIN_PASSWORD", originalEnvValue);
        }

        var any = await context.Users.AnyAsync();
        Assert.False(any);
    }

    [Fact]
    public async Task SeedAdminAsync_WithNullContext_ShouldThrowArgumentNullException()
    {
        var configuration = CreateConfiguration();

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => RepairShopSeeder.SeedAdminAsync(null!, configuration));
    }

    [Fact]
    public async Task SeedAdminAsync_WithNullConfiguration_ShouldThrowArgumentNullException()
    {
        await using var context = CreateContext();

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => RepairShopSeeder.SeedAdminAsync(context, null!));
    }
}
