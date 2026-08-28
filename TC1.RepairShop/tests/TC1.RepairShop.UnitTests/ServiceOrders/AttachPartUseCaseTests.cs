using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.ServiceOrders.UseCases;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.ServiceOrders;

public class AttachPartUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenServiceOrderAndPartExistAndNotYetAttached()
    {
        var order = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());
        var part = Part.Create("Brake Pad", 19.99m, stockQuantity: 5);
        var orderRepository = new Mock<IServiceOrderRepository>();
        var partRepository = new Mock<IPartRepository>();
        var orderPartRepository = new Mock<IServiceOrderPartRepository>();
        orderRepository.Setup(r => r.GetByIdAsync(order.Id)).ReturnsAsync(order);
        partRepository.Setup(r => r.GetByIdAsync(part.Id)).ReturnsAsync(part);
        orderRepository.Setup(r => r.GetServiceOrderPartById(order.Id, part.Id)).ReturnsAsync((ServiceOrderPart?)null);
        orderRepository.Setup(r => r.GetByIdDetailedAsync(order.Id)).ReturnsAsync(order);

        var useCase = new AttachPartUseCase(orderRepository.Object, partRepository.Object, orderPartRepository.Object);
        var result = await useCase.ExecuteAsync(new AttachPartRequest(order.Id, part.Id, 2, 19.99m, false));

        result.success.Should().BeTrue();
        orderPartRepository.Verify(r => r.AddAsync(It.IsAny<ServiceOrderPart>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenServiceOrderDoesNotExist()
    {
        var orderRepository = new Mock<IServiceOrderRepository>();
        var partRepository = new Mock<IPartRepository>();
        var orderPartRepository = new Mock<IServiceOrderPartRepository>();
        orderRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ServiceOrder?)null);

        var useCase = new AttachPartUseCase(orderRepository.Object, partRepository.Object, orderPartRepository.Object);
        var result = await useCase.ExecuteAsync(new AttachPartRequest(Guid.NewGuid(), Guid.NewGuid(), 2, 19.99m, false));

        result.success.Should().BeFalse();
        result.error.Should().Be("Service order not found.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenPartDoesNotExist()
    {
        var order = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());
        var orderRepository = new Mock<IServiceOrderRepository>();
        var partRepository = new Mock<IPartRepository>();
        var orderPartRepository = new Mock<IServiceOrderPartRepository>();
        orderRepository.Setup(r => r.GetByIdAsync(order.Id)).ReturnsAsync(order);
        partRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Part?)null);

        var useCase = new AttachPartUseCase(orderRepository.Object, partRepository.Object, orderPartRepository.Object);
        var result = await useCase.ExecuteAsync(new AttachPartRequest(order.Id, Guid.NewGuid(), 2, 19.99m, false));

        result.success.Should().BeFalse();
        result.error.Should().Be("Part not found.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenPartAlreadyAttached()
    {
        var order = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());
        var part = Part.Create("Brake Pad", 19.99m, stockQuantity: 5);
        var existingAttachment = ServiceOrderPart.Create(order.Id, part.Id, 1, 19.99m, false);
        var orderRepository = new Mock<IServiceOrderRepository>();
        var partRepository = new Mock<IPartRepository>();
        var orderPartRepository = new Mock<IServiceOrderPartRepository>();
        orderRepository.Setup(r => r.GetByIdAsync(order.Id)).ReturnsAsync(order);
        partRepository.Setup(r => r.GetByIdAsync(part.Id)).ReturnsAsync(part);
        orderRepository.Setup(r => r.GetServiceOrderPartById(order.Id, part.Id)).ReturnsAsync(existingAttachment);

        var useCase = new AttachPartUseCase(orderRepository.Object, partRepository.Object, orderPartRepository.Object);
        var result = await useCase.ExecuteAsync(new AttachPartRequest(order.Id, part.Id, 2, 19.99m, false));

        result.success.Should().BeFalse();
        result.error.Should().Be("Part is already registered.");
        orderPartRepository.Verify(r => r.AddAsync(It.IsAny<ServiceOrderPart>()), Times.Never);
    }
}
