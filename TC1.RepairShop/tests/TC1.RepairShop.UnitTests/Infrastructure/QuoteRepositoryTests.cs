using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Infrastructure.Data;
using TC1.RepairShop.Infrastructure.Data.Repositories;
using Xunit;

namespace TC1.RepairShop.UnitTests.Infrastructure;

public class QuoteRepositoryTests
{
    private static RepairShopDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<RepairShopDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new RepairShopDbContext(options, Mock.Of<IPublisher>());
    }

    private static async Task<ServiceOrder> CreateServiceOrderAsync(RepairShopDbContext context)
    {
        var unique = Guid.NewGuid().ToString("N")[..8];
        var plateDigits = (Math.Abs(unique.GetHashCode()) % 10000).ToString("D4");
        var user = User.Create($"mechanic_{unique}", "Password@123", "01098843371", $"mechanic_{unique}@example.com", UserRole.Staff, "11999999999");
        var vehicle = Vehicle.Create(user.Id, $"ABC{plateDigits}", "Ford", "Ka", 2020);
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        await context.SaveChangesAsync();

        var order = ServiceOrder.Create(user.Id, vehicle.Id);
        context.ServiceOrders.Add(order);
        await context.SaveChangesAsync();

        return order;
    }

    [Fact]
    public async Task AddAsync_ThenGetByIdAsync_ShouldReturnQuote()
    {
        await using var context = CreateContext();
        var repository = new QuoteRepository(context);
        var order = await CreateServiceOrderAsync(context);

        var quote = Quote.Create(order.Id, 150.75m);
        await repository.AddAsync(quote);

        var stored = await repository.GetByIdAsync(quote.Id);

        Assert.NotNull(stored);
        Assert.Equal(quote.Id, stored!.Id);
        Assert.Equal(order.Id, stored.ServiceOrderId);
        Assert.Equal(150.75m, stored.Price);
    }

    [Fact]
    public async Task GetByIdAsync_WithUnknownId_ShouldReturnNull()
    {
        await using var context = CreateContext();
        var repository = new QuoteRepository(context);

        var stored = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(stored);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllQuotes()
    {
        await using var context = CreateContext();
        var repository = new QuoteRepository(context);
        var order1 = await CreateServiceOrderAsync(context);
        var order2 = await CreateServiceOrderAsync(context);

        var quote1 = Quote.Create(order1.Id, 100m);
        var quote2 = Quote.Create(order2.Id, 200m);
        context.Quotes.Add(quote1);
        context.Quotes.Add(quote2);
        await context.SaveChangesAsync();

        var all = await repository.GetAllAsync();

        Assert.Equal(2, all.Count());
    }

    [Fact]
    public async Task GetAllAsync_WithNoQuotes_ShouldReturnEmpty()
    {
        await using var context = CreateContext();
        var repository = new QuoteRepository(context);

        var all = await repository.GetAllAsync();

        Assert.Empty(all);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        await using var context = CreateContext();
        var repository = new QuoteRepository(context);
        var order = await CreateServiceOrderAsync(context);

        var quote = Quote.Create(order.Id, 100m);
        context.Quotes.Add(quote);
        await context.SaveChangesAsync();

        quote.UpdatePrice(250m);
        await repository.UpdateAsync(quote);

        var stored = await repository.GetByIdAsync(quote.Id);
        Assert.NotNull(stored);
        Assert.Equal(250m, stored!.Price);
    }

    [Fact]
    public async Task DeleteAsync_WithUnknownId_ShouldNotThrow()
    {
        await using var context = CreateContext();
        var repository = new QuoteRepository(context);

        await repository.DeleteAsync(Guid.NewGuid());

        Assert.True(true);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrueForStoredEntity()
    {
        await using var context = CreateContext();
        var repository = new QuoteRepository(context);
        var order = await CreateServiceOrderAsync(context);

        var quote = Quote.Create(order.Id, 100m);
        context.Quotes.Add(quote);
        await context.SaveChangesAsync();

        var exists = await repository.ExistsAsync(quote.Id);
        var notExists = await repository.ExistsAsync(Guid.NewGuid());

        Assert.True(exists);
        Assert.False(notExists);
    }
}
