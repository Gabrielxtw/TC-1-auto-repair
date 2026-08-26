using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Infrastructure.Data;
using TC1.RepairShop.Infrastructure.Data.Repositories;
using Xunit;

namespace TC1.RepairShop.UnitTests.Infrastructure;

public class ServiceOrderPartRepositoryTests
{
    private static RepairShopDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<RepairShopDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new RepairShopDbContext(options, Mock.Of<IPublisher>());
    }

    private static (User user, Vehicle vehicle, ServiceOrder order, Part part) CreateOrderAndPart()
    {
        var user = User.Create("mechanic1", "Password@123", "01098843371", "mechanic1@example.com", UserRole.Staff, "11999999999");
        var vehicle = Vehicle.Create(user.Id, "ABC1D23", "Ford", "Ka", 2020);
        var order = ServiceOrder.Create(user.Id, vehicle.Id);
        var part = Part.Create("Brake Pad", 150.00m, 10);
        return (user, vehicle, order, part);
    }

    [Fact]
    public async Task AddAsync_ThenGetByIdAsync_ShouldReturnServiceOrderPart()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderPartRepository(context);
        var (user, vehicle, order, part) = CreateOrderAndPart();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        context.ServiceOrders.Add(order);
        context.Parts.Add(part);
        await context.SaveChangesAsync();

        var serviceOrderPart = ServiceOrderPart.Create(order.Id, part.Id, 2, 150.00m, false);
        await repository.AddAsync(serviceOrderPart);

        var stored = await repository.GetByIdAsync(serviceOrderPart.Id);

        Assert.NotNull(stored);
        Assert.Equal(serviceOrderPart.Id, stored!.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllServiceOrderParts()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderPartRepository(context);
        var (user, vehicle, order, part) = CreateOrderAndPart();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        context.ServiceOrders.Add(order);
        context.Parts.Add(part);
        var serviceOrderPart = ServiceOrderPart.Create(order.Id, part.Id, 2, 150.00m, false);
        context.ServiceOrderParts.Add(serviceOrderPart);
        await context.SaveChangesAsync();

        var all = await repository.GetAllAsync();

        var result = Assert.Single(all);
        Assert.Equal(serviceOrderPart.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WithUnknownId_ShouldReturnNull()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderPartRepository(context);

        var result = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderPartRepository(context);
        var (user, vehicle, order, part) = CreateOrderAndPart();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        context.ServiceOrders.Add(order);
        context.Parts.Add(part);
        var serviceOrderPart = ServiceOrderPart.Create(order.Id, part.Id, 2, 150.00m, false);
        context.ServiceOrderParts.Add(serviceOrderPart);
        await context.SaveChangesAsync();

        typeof(ServiceOrderPart).GetProperty("Quantity")!.SetValue(serviceOrderPart, 5);
        typeof(ServiceOrderPart).GetProperty("Price")!.SetValue(serviceOrderPart, 200.00m);
        typeof(ServiceOrderPart).GetProperty("SuppliedByCustomer")!.SetValue(serviceOrderPart, true);
        await repository.UpdateAsync(serviceOrderPart);

        var stored = await repository.GetByIdAsync(serviceOrderPart.Id);
        Assert.NotNull(stored);
        Assert.Equal(5, stored!.Quantity);
        Assert.Equal(200.00m, stored.Price);
        Assert.True(stored.SuppliedByCustomer);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrueForStoredEntity()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderPartRepository(context);
        var (user, vehicle, order, part) = CreateOrderAndPart();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        context.ServiceOrders.Add(order);
        context.Parts.Add(part);
        var serviceOrderPart = ServiceOrderPart.Create(order.Id, part.Id, 2, 150.00m, false);
        context.ServiceOrderParts.Add(serviceOrderPart);
        await context.SaveChangesAsync();

        var exists = await repository.ExistsAsync(serviceOrderPart.Id);
        var notExists = await repository.ExistsAsync(Guid.NewGuid());

        Assert.True(exists);
        Assert.False(notExists);
    }

    [Fact]
    public async Task DeleteAsync_WithUnknownId_ShouldNotThrow()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderPartRepository(context);

        await repository.DeleteAsync(Guid.NewGuid());

        Assert.True(true);
    }

    [Fact]
    public async Task GetByServiceOrderIdAsync_ShouldReturnMatchingParts()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderPartRepository(context);
        var (user, vehicle, order, part) = CreateOrderAndPart();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        context.ServiceOrders.Add(order);
        context.Parts.Add(part);
        var serviceOrderPart = ServiceOrderPart.Create(order.Id, part.Id, 2, 150.00m, false);
        context.ServiceOrderParts.Add(serviceOrderPart);
        await context.SaveChangesAsync();

        var result = (await repository.GetByServiceOrderIdAsync(order.Id)).ToList();

        var item = Assert.Single(result);
        Assert.Equal(serviceOrderPart.Id, item.Id);
        Assert.NotNull(item.Part);
        Assert.Equal(part.Id, item.Part.Id);
    }

    [Fact]
    public async Task GetByServiceOrderIdAsync_WithUnknownOrder_ShouldReturnEmpty()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderPartRepository(context);

        var result = await repository.GetByServiceOrderIdAsync(Guid.NewGuid());

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByServiceOrderAndPartIdAsync_ShouldReturnMatchingPart()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderPartRepository(context);
        var (user, vehicle, order, part) = CreateOrderAndPart();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        context.ServiceOrders.Add(order);
        context.Parts.Add(part);
        var serviceOrderPart = ServiceOrderPart.Create(order.Id, part.Id, 2, 150.00m, false);
        context.ServiceOrderParts.Add(serviceOrderPart);
        await context.SaveChangesAsync();

        var result = await repository.GetByServiceOrderAndPartIdAsync(order.Id, part.Id);

        Assert.NotNull(result);
        Assert.Equal(serviceOrderPart.Id, result!.Id);
        Assert.NotNull(result.Part);
        Assert.Equal(part.Id, result.Part.Id);
    }

    [Fact]
    public async Task GetByServiceOrderAndPartIdAsync_WithUnknownCombination_ShouldReturnNull()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderPartRepository(context);
        var (user, vehicle, order, part) = CreateOrderAndPart();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        context.ServiceOrders.Add(order);
        context.Parts.Add(part);
        var serviceOrderPart = ServiceOrderPart.Create(order.Id, part.Id, 2, 150.00m, false);
        context.ServiceOrderParts.Add(serviceOrderPart);
        await context.SaveChangesAsync();

        var result = await repository.GetByServiceOrderAndPartIdAsync(order.Id, Guid.NewGuid());

        Assert.Null(result);
    }
}
