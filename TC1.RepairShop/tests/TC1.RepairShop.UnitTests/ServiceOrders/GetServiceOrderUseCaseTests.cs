using System.Reflection;
using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.ServiceOrders.UseCases;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.ServiceOrders;

public class GetServiceOrderUseCaseTests
{
    private static void SetNavigation<TEntity, TValue>(TEntity entity, string propertyName, TValue value)
    {
        typeof(TEntity).GetProperty(propertyName)!.SetValue(entity, value);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnMappedServiceOrder_WhenFound()
    {
        var user = User.Create("alice", "Passw0rd!", "52998224725", "alice@example.com", UserRole.Staff, "1999999999");
        var vehicle = Vehicle.Create(user.Id, "ABC1234", "Toyota", "Corolla", 2022);
        var order = ServiceOrder.Create(user.Id, vehicle.Id);
        SetNavigation<ServiceOrder, User>(order, nameof(ServiceOrder.User), user);
        SetNavigation<ServiceOrder, Vehicle>(order, nameof(ServiceOrder.Vehicle), vehicle);

        var service = Service.Create("Oil Change", "Change engine oil", 59.99m);
        var orderService = ServiceOrderService.Create(order.Id, service.Id, 59.99m);
        SetNavigation<ServiceOrderService, Service>(orderService, nameof(ServiceOrderService.Service), service);
        order.ServiceOrderServices.Add(orderService);

        var part = Part.Create("Brake Pad", 19.99m, stockQuantity: 5);
        var orderPart = ServiceOrderPart.Create(order.Id, part.Id, 2, 19.99m, false);
        SetNavigation<ServiceOrderPart, Part>(orderPart, nameof(ServiceOrderPart.Part), part);
        order.ServiceOrderParts.Add(orderPart);

        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.GetByIdDetailedAsync(order.Id)).ReturnsAsync(order);

        var useCase = new GetServiceOrderUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(order.Id);

        result.success.Should().BeTrue();
        result.data!.Id.Should().Be(order.Id);
        result.data.User.Username.Should().Be("alice");
        result.data.Vehicle.LicensePlate.Should().Be(vehicle.LicensePlate.ToString());
        result.data.Quote.Should().BeNull();
        result.data.Services.Should().ContainSingle(s => s.Service.Name == "Oil Change");
        result.data.Parts.Should().ContainSingle(p => p.Part.Name == "Brake Pad");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenServiceOrderDoesNotExist()
    {
        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.GetByIdDetailedAsync(It.IsAny<Guid>())).ReturnsAsync((ServiceOrder?)null);

        var useCase = new GetServiceOrderUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(Guid.NewGuid());

        result.success.Should().BeFalse();
        result.error.Should().Be("Service order not found.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenRepositoryThrowsUnexpectedException()
    {
        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.GetByIdDetailedAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("db unavailable"));

        var useCase = new GetServiceOrderUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(Guid.NewGuid());

        result.success.Should().BeFalse();
        result.error.Should().Be("db unavailable");
    }
}
