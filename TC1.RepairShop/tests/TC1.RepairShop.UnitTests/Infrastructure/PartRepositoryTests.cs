using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Infrastructure.Data;
using TC1.RepairShop.Infrastructure.Data.Repositories;
using Xunit;

namespace TC1.RepairShop.UnitTests.Infrastructure;

public class PartRepositoryTests
{
    private static RepairShopDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<RepairShopDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new RepairShopDbContext(options, Mock.Of<IPublisher>());
    }

    [Fact]
    public async Task AddAsync_ThenGetByIdAsync_ShouldReturnPart()
    {
        await using var context = CreateContext();
        var repository = new PartRepository(context);
        var part = Part.Create("Oil Filter", 25.90m, 10);

        await repository.AddAsync(part);

        var stored = await repository.GetByIdAsync(part.Id);

        Assert.NotNull(stored);
        Assert.Equal(part.Id, stored!.Id);
        Assert.Equal("Oil Filter", stored.Name);
        Assert.Equal(25.90m, stored.Price);
        Assert.Equal(10, stored.StockQuantity);
    }

    [Fact]
    public async Task GetByIdAsync_WithUnknownId_ShouldReturnNull()
    {
        await using var context = CreateContext();
        var repository = new PartRepository(context);

        var stored = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(stored);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllParts()
    {
        await using var context = CreateContext();
        var repository = new PartRepository(context);
        var part1 = Part.Create("Oil Filter", 25.90m, 10);
        var part2 = Part.Create("Brake Pad", 89.50m, 5);
        context.Parts.Add(part1);
        context.Parts.Add(part2);
        await context.SaveChangesAsync();

        var all = await repository.GetAllAsync();

        Assert.Equal(2, all.Count());
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        await using var context = CreateContext();
        var repository = new PartRepository(context);
        var part = Part.Create("Oil Filter", 25.90m, 10);
        context.Parts.Add(part);
        await context.SaveChangesAsync();

        part.Update("Oil Filter Premium", 32.00m);
        await repository.UpdateAsync(part);

        var stored = await repository.GetByIdAsync(part.Id);
        Assert.NotNull(stored);
        Assert.Equal("Oil Filter Premium", stored!.Name);
        Assert.Equal(32.00m, stored.Price);
    }

    [Fact]
    public async Task DeleteAsync_WithUnknownId_ShouldNotThrow()
    {
        await using var context = CreateContext();
        var repository = new PartRepository(context);

        await repository.DeleteAsync(Guid.NewGuid());

        Assert.True(true);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrueForStoredEntity()
    {
        await using var context = CreateContext();
        var repository = new PartRepository(context);
        var part = Part.Create("Oil Filter", 25.90m, 10);
        context.Parts.Add(part);
        await context.SaveChangesAsync();

        var exists = await repository.ExistsAsync(part.Id);
        var notExists = await repository.ExistsAsync(Guid.NewGuid());

        Assert.True(exists);
        Assert.False(notExists);
    }

    [Fact]
    public async Task ExistsByNameAsync_ShouldReturnTrue_WhenNameExists()
    {
        await using var context = CreateContext();
        var repository = new PartRepository(context);
        var part = Part.Create("Oil Filter", 25.90m, 10);
        context.Parts.Add(part);
        await context.SaveChangesAsync();

        var exists = await repository.ExistsByNameAsync("Oil Filter");

        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsByNameAsync_ShouldReturnFalse_WhenNameDoesNotExist()
    {
        await using var context = CreateContext();
        var repository = new PartRepository(context);
        var part = Part.Create("Oil Filter", 25.90m, 10);
        context.Parts.Add(part);
        await context.SaveChangesAsync();

        var exists = await repository.ExistsByNameAsync("Brake Pad");

        Assert.False(exists);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ExistsByNameAsync_ShouldReturnFalse_WhenNameIsNullOrWhitespace(string? name)
    {
        await using var context = CreateContext();
        var repository = new PartRepository(context);

        var exists = await repository.ExistsByNameAsync(name!);

        Assert.False(exists);
    }
}
