using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Infrastructure.Data;
using TC1.RepairShop.Infrastructure.Data.Repositories;
using Xunit;

namespace TC1.RepairShop.UnitTests.Infrastructure;

public class ServiceOrderServiceRepositoryTests
{
    private static RepairShopDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<RepairShopDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new RepairShopDbContext(options, Mock.Of<IPublisher>());
    }

    private static (User user, Vehicle vehicle, ServiceOrder order, Service service) CreateOrderAndService()
    {
        var user = User.Create("mechanic1", "Password@123", "01098843371", "mechanic1@example.com", UserRole.Staff, "11999999999");
        var vehicle = Vehicle.Create(user.Id, "ABC1D23", "Ford", "Ka", 2020);
        var order = ServiceOrder.Create(user.Id, vehicle.Id);
        var service = Service.Create("Oil Change", "Change engine oil", 150m);
        return (user, vehicle, order, service);
    }

    [Fact]
    public async Task AddAsync_ThenGetByIdAsync_ShouldReturnServiceOrderService()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderServiceRepository(context);
        var (user, vehicle, order, service) = CreateOrderAndService();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        context.ServiceOrders.Add(order);
        context.Services.Add(service);
        await context.SaveChangesAsync();

        var sos = ServiceOrderService.Create(order.Id, service.Id, 150m);
        await repository.AddAsync(sos);

        var stored = await repository.GetByIdAsync(sos.Id);

        Assert.NotNull(stored);
        Assert.Equal(sos.Id, stored!.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllServiceOrderServices()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderServiceRepository(context);
        var (user, vehicle, order, service) = CreateOrderAndService();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        context.ServiceOrders.Add(order);
        context.Services.Add(service);
        var sos = ServiceOrderService.Create(order.Id, service.Id, 150m);
        context.ServiceOrderServices.Add(sos);
        await context.SaveChangesAsync();

        var all = await repository.GetAllAsync();

        var result = Assert.Single(all);
        Assert.Equal(sos.Id, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderServiceRepository(context);
        var (user, vehicle, order, service) = CreateOrderAndService();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        context.ServiceOrders.Add(order);
        context.Services.Add(service);
        var sos = ServiceOrderService.Create(order.Id, service.Id, 150m);
        context.ServiceOrderServices.Add(sos);
        await context.SaveChangesAsync();

        await repository.UpdateAsync(sos);

        var stored = await repository.GetByIdAsync(sos.Id);
        Assert.NotNull(stored);
        Assert.Equal(sos.Price, stored!.Price);
    }

    [Fact]
    public async Task DeleteAsync_WithUnknownId_ShouldNotThrow()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderServiceRepository(context);

        await repository.DeleteAsync(Guid.NewGuid());

        Assert.True(true);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrueForStoredEntity()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderServiceRepository(context);
        var (user, vehicle, order, service) = CreateOrderAndService();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        context.ServiceOrders.Add(order);
        context.Services.Add(service);
        var sos = ServiceOrderService.Create(order.Id, service.Id, 150m);
        context.ServiceOrderServices.Add(sos);
        await context.SaveChangesAsync();

        var exists = await repository.ExistsAsync(sos.Id);
        var notExists = await repository.ExistsAsync(Guid.NewGuid());

        Assert.True(exists);
        Assert.False(notExists);
    }

    [Fact]
    public async Task GetByServiceOrderIdAsync_ShouldReturnMatchingEntriesWithService()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderServiceRepository(context);
        var (user, vehicle, order, service) = CreateOrderAndService();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        context.ServiceOrders.Add(order);
        context.Services.Add(service);
        var sos = ServiceOrderService.Create(order.Id, service.Id, 150m);
        context.ServiceOrderServices.Add(sos);
        await context.SaveChangesAsync();

        var result = await repository.GetByServiceOrderIdAsync(order.Id);

        var single = Assert.Single(result);
        Assert.Equal(sos.Id, single.Id);
        Assert.NotNull(single.Service);
        Assert.Equal(service.Id, single.Service.Id);
    }

    [Fact]
    public async Task GetByServiceOrderIdAsync_WithUnknownOrder_ShouldReturnEmpty()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderServiceRepository(context);

        var result = await repository.GetByServiceOrderIdAsync(Guid.NewGuid());

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByServiceOrderAndServiceIdAsync_ShouldReturnMatchingEntryWithService()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderServiceRepository(context);
        var (user, vehicle, order, service) = CreateOrderAndService();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        context.ServiceOrders.Add(order);
        context.Services.Add(service);
        var sos = ServiceOrderService.Create(order.Id, service.Id, 150m);
        context.ServiceOrderServices.Add(sos);
        await context.SaveChangesAsync();

        var result = await repository.GetByServiceOrderAndServiceIdAsync(order.Id, service.Id);

        Assert.NotNull(result);
        Assert.Equal(sos.Id, result!.Id);
        Assert.NotNull(result.Service);
        Assert.Equal(service.Id, result.Service.Id);
    }

    [Fact]
    public async Task GetByServiceOrderAndServiceIdAsync_WithUnknownCombination_ShouldReturnNull()
    {
        await using var context = CreateContext();
        var repository = new ServiceOrderServiceRepository(context);
        var (user, vehicle, order, service) = CreateOrderAndService();
        context.Users.Add(user);
        context.Vehicles.Add(vehicle);
        context.ServiceOrders.Add(order);
        context.Services.Add(service);
        var sos = ServiceOrderService.Create(order.Id, service.Id, 150m);
        context.ServiceOrderServices.Add(sos);
        await context.SaveChangesAsync();

        var result = await repository.GetByServiceOrderAndServiceIdAsync(order.Id, Guid.NewGuid());

        Assert.Null(result);
    }
}
