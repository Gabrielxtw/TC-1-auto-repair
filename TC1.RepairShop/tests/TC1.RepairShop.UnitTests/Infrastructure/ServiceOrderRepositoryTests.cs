using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Infrastructure.Data;
using TC1.RepairShop.Infrastructure.Data.Repositories;
using Xunit;

namespace TC1.RepairShop.UnitTests.Infrastructure;

public class ServiceOrderRepositoryTests
{
    private static RepairShopDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<RepairShopDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new RepairShopDbContext(options, Mock.Of<IPublisher>());
    }

    private static (User user, Vehicle vehicle) CreateUserAndVehicle()
    {
        var user = User.Create("mechanic1", "Password@123", "01098843371", "mechanic1@example.com", UserRole.Staff, "11999999999");
        var vehicle = Vehicle.Create(user.Id, "ABC1D23", "Ford", "Ka", 2020);
        return (user, vehicle);
    }

    [Fact]
    public async Task AddAsync_ThenGetByIdAsync_ShouldReturnServiceOrder()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderRepository(context);
        var (user, vehicle) = CreateUserAndVehicle();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        await context.SaveChangesAsync();

        var order = ServiceOrder.Create(user.Id, vehicle.Id);
        await repository.AddAsync(order);

        var stored = await repository.GetByIdAsync(order.Id);

        Assert.NotNull(stored);
        Assert.Equal(order.Id, stored!.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldIncludeUser()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderRepository(context);
        var (user, vehicle) = CreateUserAndVehicle();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        var order = ServiceOrder.Create(user.Id, vehicle.Id);
        context.ServiceOrders.Add(order);
        await context.SaveChangesAsync();

        var all = await repository.GetAllAsync();

        var result = Assert.Single(all);
        Assert.NotNull(result.User);
        Assert.Equal(user.Id, result.User.Id);
    }

    [Fact]
    public async Task GetByIdDetailedAsync_ShouldReturnOrderWithRelatedData()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderRepository(context);
        var (user, vehicle) = CreateUserAndVehicle();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        var order = ServiceOrder.Create(user.Id, vehicle.Id);
        context.ServiceOrders.Add(order);
        await context.SaveChangesAsync();

        var detailed = await repository.GetByIdDetailedAsync(order.Id);

        Assert.NotNull(detailed);
        Assert.Equal(vehicle.Id, detailed!.VehicleId);
    }

    [Fact]
    public async Task GetByIdDetailedAsync_WithUnknownId_ShouldReturnNull()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderRepository(context);

        var detailed = await repository.GetByIdDetailedAsync(Guid.NewGuid());

        Assert.Null(detailed);
    }

    [Fact]
    public async Task GetByUserId_ShouldReturnMatchingOrder()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderRepository(context);
        var (user, vehicle) = CreateUserAndVehicle();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        var order = ServiceOrder.Create(user.Id, vehicle.Id);
        context.ServiceOrders.Add(order);
        await context.SaveChangesAsync();

        var result = await repository.GetByUserId(user.Id);

        Assert.Equal(order.Id, result.Id);
    }

    [Fact]
    public async Task GetByUserId_WithUnknownUser_ShouldThrow()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderRepository(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetByUserId(Guid.NewGuid()));
    }

    [Fact]
    public async Task GetServiceOrderServiceById_WithUnknownOrder_ShouldThrow()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderRepository(context);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => repository.GetServiceOrderServiceById(Guid.NewGuid(), Guid.NewGuid()));
    }

    [Fact]
    public async Task GetServiceOrderPartById_WithUnknownOrder_ShouldThrow()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderRepository(context);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => repository.GetServiceOrderPartById(Guid.NewGuid(), Guid.NewGuid()));
    }

    [Fact]
    public async Task GetServiceOrderServiceById_WithOrderButNoService_ShouldReturnNull()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderRepository(context);
        var (user, vehicle) = CreateUserAndVehicle();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        var order = ServiceOrder.Create(user.Id, vehicle.Id);
        context.ServiceOrders.Add(order);
        await context.SaveChangesAsync();

        var result = await repository.GetServiceOrderServiceById(order.Id, Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_WithUnknownId_ShouldReturnWithoutThrowing()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderRepository(context);

        await repository.DeleteAsync(Guid.NewGuid());

        Assert.True(true);
    }

    [Fact]
    public async Task DeleteAsync_ShouldMarkStatusDeleted()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderRepository(context);
        var (user, vehicle) = CreateUserAndVehicle();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        var order = ServiceOrder.Create(user.Id, vehicle.Id);
        context.ServiceOrders.Add(order);
        await context.SaveChangesAsync();

        await repository.DeleteAsync(order.Id);

        var stored = await repository.GetByIdAsync(order.Id);
        Assert.Equal(Status.Deleted, stored!.Status);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrueForStoredEntity()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderRepository(context);
        var (user, vehicle) = CreateUserAndVehicle();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        var order = ServiceOrder.Create(user.Id, vehicle.Id);
        context.ServiceOrders.Add(order);
        await context.SaveChangesAsync();

        var exists = await repository.ExistsAsync(order.Id);
        var notExists = await repository.ExistsAsync(Guid.NewGuid());

        Assert.True(exists);
        Assert.False(notExists);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderRepository(context);
        var (user, vehicle) = CreateUserAndVehicle();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        var order = ServiceOrder.Create(user.Id, vehicle.Id);
        context.ServiceOrders.Add(order);
        await context.SaveChangesAsync();

        order.AttachQuote(Guid.NewGuid());
        await repository.UpdateAsync(order);

        var stored = await repository.GetByIdAsync(order.Id);
        Assert.NotNull(stored!.QuoteId);
    }
}
