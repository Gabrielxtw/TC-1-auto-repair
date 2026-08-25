using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.ServiceOrders.UseCases;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.ServiceOrders;

public class CancelServiceOrderUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenOrderIsCancellable()
    {
        var order = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());
        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.GetByIdAsync(order.Id)).ReturnsAsync(order);

        var useCase = new CancelServiceOrderUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new CancelServiceOrderRequest(order.Id));

        result.success.Should().BeTrue();
        order.OrderStatusValue.Should().Be(ServiceOrderStatus.Cancelled);
        repository.Verify(r => r.UpdateAsync(order), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenServiceOrderDoesNotExist()
    {
        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ServiceOrder?)null);

        var useCase = new CancelServiceOrderUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new CancelServiceOrderRequest(Guid.NewGuid()));

        result.success.Should().BeFalse();
        result.error.Should().Be("Service order not found.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenOrderIsAlreadyInTerminalStatus()
    {
        var order = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());
        order.AdvanceTo(ServiceOrderStatus.Cancelled);
        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.GetByIdAsync(order.Id)).ReturnsAsync(order);

        var useCase = new CancelServiceOrderUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new CancelServiceOrderRequest(order.Id));

        result.success.Should().BeFalse();
        result.error.Should().Be("Cannot transition from the current status to the new status.");
    }
}
