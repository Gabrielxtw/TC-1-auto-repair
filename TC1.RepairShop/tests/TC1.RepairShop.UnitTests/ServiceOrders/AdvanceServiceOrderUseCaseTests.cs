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

public class AdvanceServiceOrderUseCaseTests
{
    private static void AttachUser(ServiceOrder order, User user)
    {
        typeof(ServiceOrder).GetProperty(nameof(ServiceOrder.User))!
            .SetValue(order, user);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenTransitionIsValid()
    {
        var user = User.Create("alice", "Passw0rd!", "52998224725", "alice@example.com", UserRole.Staff, "1999999999");
        var order = ServiceOrder.Create(user.Id, Guid.NewGuid());
        AttachUser(order, user);
        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.GetByIdDetailedAsync(order.Id)).ReturnsAsync(order);

        var useCase = new AdvanceServiceOrderUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new AdvanceServiceOrderRequest(order.Id, ServiceOrderStatus.UnderDiagnosis.Name));

        result.success.Should().BeTrue();
        order.OrderStatusValue.Should().Be(ServiceOrderStatus.UnderDiagnosis);
        repository.Verify(r => r.UpdateAsync(order), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenServiceOrderDoesNotExist()
    {
        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.GetByIdDetailedAsync(It.IsAny<Guid>())).ReturnsAsync((ServiceOrder?)null);

        var useCase = new AdvanceServiceOrderUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new AdvanceServiceOrderRequest(Guid.NewGuid(), ServiceOrderStatus.UnderDiagnosis.Name));

        result.success.Should().BeFalse();
        result.error.Should().Be("Service order not found.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenTransitionIsInvalid()
    {
        var order = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());
        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.GetByIdDetailedAsync(order.Id)).ReturnsAsync(order);

        var useCase = new AdvanceServiceOrderUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new AdvanceServiceOrderRequest(order.Id, ServiceOrderStatus.Delivered.Name));

        result.success.Should().BeFalse();
        result.error.Should().Be("Cannot transition from the current status to the new status.");
        repository.Verify(r => r.UpdateAsync(It.IsAny<ServiceOrder>()), Times.Never);
    }
}
