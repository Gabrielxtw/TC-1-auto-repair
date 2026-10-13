using System.Reflection;
using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.ServiceOrders.UseCases;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.ServiceOrders;

public class ListServiceOrdersUseCaseTests
{
    private static void AttachUser(ServiceOrder order, User user)
    {
        typeof(ServiceOrder).GetProperty(nameof(ServiceOrder.User))!
            .SetValue(order, user);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnMappedServiceOrders_WhenRepositorySucceeds()
    {
        var user = User.Create("alice", "Passw0rd!", "52998224725", "alice@example.com", UserRole.Staff, "1999999999");
        var order = ServiceOrder.Create(user.Id, Guid.NewGuid());
        AttachUser(order, user);
        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.GetFilteredAsync(null, null, null, true)).ReturnsAsync(new[] { order });

        var useCase = new ListServiceOrdersUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new ListServiceOrdersRequest(null, null, null));

        result.success.Should().BeTrue();
        result.data.Orders.Should().ContainSingle(r =>
            r.Id == order.Id &&
            r.CustomerName == "alice" &&
            r.CustomerEmail == "alice@example.com");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnEmptyFailure_WhenRepositoryThrows()
    {
        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.GetFilteredAsync(null, null, null, true)).ThrowsAsync(new Exception("db unavailable"));

        var useCase = new ListServiceOrdersUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new ListServiceOrdersRequest(null, null, null));

        result.success.Should().BeFalse();
        result.data.Should().BeNull();
        result.error.Should().Be("db unavailable");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldForwardFiltersAndSortDirectionToRepository()
    {
        var repository = new Mock<IServiceOrderRepository>();
        var openedFrom = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var openedTo = new DateTime(2025, 1, 31, 23, 59, 59, DateTimeKind.Utc);
        var request = new ListServiceOrdersRequest(ServiceOrderStatus.Received, openedFrom, openedTo, false);
        repository.Setup(r => r.GetFilteredAsync(request.Status, openedFrom, openedTo, false))
            .ReturnsAsync(Array.Empty<ServiceOrder>());

        var useCase = new ListServiceOrdersUseCase(repository.Object);
        await useCase.ExecuteAsync(request);

        repository.Verify(r => r.GetFilteredAsync(ServiceOrderStatus.Received, openedFrom, openedTo, false), Times.Once);
    }
}
