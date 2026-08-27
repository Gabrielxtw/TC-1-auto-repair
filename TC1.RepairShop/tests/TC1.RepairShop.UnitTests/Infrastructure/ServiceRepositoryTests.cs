using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Infrastructure.Data;
using TC1.RepairShop.Infrastructure.Data.Repositories;
using Xunit;

namespace TC1.RepairShop.UnitTests.Infrastructure;

public class ServiceRepositoryTests
{
    private static RepairShopDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<RepairShopDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new RepairShopDbContext(options, Mock.Of<IPublisher>());
    }

    [Fact]
    public async Task AddAsync_ThenGetByIdAsync_ShouldReturnService()
    {
        await using var context = CreateContext();
        var repository = new ServiceRepository(context);
        var service = Service.Create("Oil Change", "Replace engine oil and filter", 150m);

        await repository.AddAsync(service);

        var stored = await repository.GetByIdAsync(service.Id);

        Assert.NotNull(stored);
        Assert.Equal(service.Id, stored!.Id);
        Assert.Equal("Oil Change", stored.Name);
        Assert.Equal("Replace engine oil and filter", stored.Description);
        Assert.Equal(150m, stored.Price);
    }

    [Fact]
    public async Task GetByIdAsync_WithUnknownId_ShouldReturnNull()
    {
        await using var context = CreateContext();
        var repository = new ServiceRepository(context);

        var stored = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(stored);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllServices()
    {
        await using var context = CreateContext();
        var repository = new ServiceRepository(context);
        var service1 = Service.Create("Oil Change", "Replace engine oil and filter", 150m);
        var service2 = Service.Create("Tire Rotation", "Rotate all four tires", 80m);
        context.Services.Add(service1);
        context.Services.Add(service2);
        await context.SaveChangesAsync();

        var all = await repository.GetAllAsync();

        Assert.Equal(2, all.Count());
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        await using var context = CreateContext();
        var repository = new ServiceRepository(context);
        var service = Service.Create("Oil Change", "Replace engine oil and filter", 150m);
        context.Services.Add(service);
        await context.SaveChangesAsync();

        service.Deactivate();
        await repository.UpdateAsync(service);

        var stored = await repository.GetByIdAsync(service.Id);
        Assert.Equal(Status.Inactive, stored!.Status);
    }

    [Fact]
    public async Task DeleteAsync_WithUnknownId_ShouldNotThrow()
    {
        await using var context = CreateContext();
        var repository = new ServiceRepository(context);
        var service = Service.Create("Oil Change", "Replace engine oil and filter", 150m);
        context.Services.Add(service);
        await context.SaveChangesAsync();

        await repository.DeleteAsync(Guid.NewGuid());

        Assert.True(true);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrueForStoredEntity()
    {
        await using var context = CreateContext();
        var repository = new ServiceRepository(context);
        var service = Service.Create("Oil Change", "Replace engine oil and filter", 150m);
        context.Services.Add(service);
        await context.SaveChangesAsync();

        var exists = await repository.ExistsAsync(service.Id);
        var notExists = await repository.ExistsAsync(Guid.NewGuid());

        Assert.True(exists);
        Assert.False(notExists);
    }

    [Fact]
    public async Task ExistsByNameAsync_WithExistingName_ShouldReturnTrue()
    {
        await using var context = CreateContext();
        var repository = new ServiceRepository(context);
        var service = Service.Create("Oil Change", "Replace engine oil and filter", 150m);
        context.Services.Add(service);
        await context.SaveChangesAsync();

        var exists = await repository.ExistsByNameAsync("Oil Change");

        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsByNameAsync_WithUnknownName_ShouldReturnFalse()
    {
        await using var context = CreateContext();
        var repository = new ServiceRepository(context);
        var service = Service.Create("Oil Change", "Replace engine oil and filter", 150m);
        context.Services.Add(service);
        await context.SaveChangesAsync();

        var exists = await repository.ExistsByNameAsync("Tire Rotation");

        Assert.False(exists);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ExistsByNameAsync_WithNullOrWhitespaceName_ShouldReturnFalse(string? name)
    {
        await using var context = CreateContext();
        var repository = new ServiceRepository(context);

        var exists = await repository.ExistsByNameAsync(name!);

        Assert.False(exists);
    }
}
