using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.ServiceOrders.UseCases;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.ServiceOrders;

public class AttachServiceUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenServiceOrderAndServiceExistAndNotYetAttached()
    {
        var order = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());
        var service = Service.Create("Oil Change", "Change engine oil", 59.99m);
        var orderRepository = new Mock<IServiceOrderRepository>();
        var serviceRepository = new Mock<IServiceRepository>();
        var orderServiceRepository = new Mock<IServiceOrderServiceRepository>();
        orderRepository.Setup(r => r.GetByIdAsync(order.Id)).ReturnsAsync(order);
        serviceRepository.Setup(r => r.GetByIdAsync(service.Id)).ReturnsAsync(service);
        orderRepository.Setup(r => r.GetServiceOrderServiceById(order.Id, service.Id)).ReturnsAsync((ServiceOrderService?)null);

        var useCase = new AttachServiceUseCase(orderRepository.Object, serviceRepository.Object, orderServiceRepository.Object);
        var result = await useCase.ExecuteAsync(new AttachServiceRequest(order.Id, service.Id, 59.99m));

        result.success.Should().BeTrue();
        orderServiceRepository.Verify(r => r.AddAsync(It.IsAny<ServiceOrderService>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenServiceOrderDoesNotExist()
    {
        var orderRepository = new Mock<IServiceOrderRepository>();
        var serviceRepository = new Mock<IServiceRepository>();
        var orderServiceRepository = new Mock<IServiceOrderServiceRepository>();
        orderRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ServiceOrder?)null);

        var useCase = new AttachServiceUseCase(orderRepository.Object, serviceRepository.Object, orderServiceRepository.Object);
        var result = await useCase.ExecuteAsync(new AttachServiceRequest(Guid.NewGuid(), Guid.NewGuid(), 59.99m));

        result.success.Should().BeFalse();
        result.error.Should().Be("Service order not found.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenServiceDoesNotExist()
    {
        var order = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());
        var orderRepository = new Mock<IServiceOrderRepository>();
        var serviceRepository = new Mock<IServiceRepository>();
        var orderServiceRepository = new Mock<IServiceOrderServiceRepository>();
        orderRepository.Setup(r => r.GetByIdAsync(order.Id)).ReturnsAsync(order);
        serviceRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Service?)null);

        var useCase = new AttachServiceUseCase(orderRepository.Object, serviceRepository.Object, orderServiceRepository.Object);
        var result = await useCase.ExecuteAsync(new AttachServiceRequest(order.Id, Guid.NewGuid(), 59.99m));

        result.success.Should().BeFalse();
        result.error.Should().Be("Service not found.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenServiceAlreadyAttached()
    {
        var order = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());
        var service = Service.Create("Oil Change", "Change engine oil", 59.99m);
        var existingAttachment = ServiceOrderService.Create(order.Id, service.Id, 59.99m);
        var orderRepository = new Mock<IServiceOrderRepository>();
        var serviceRepository = new Mock<IServiceRepository>();
        var orderServiceRepository = new Mock<IServiceOrderServiceRepository>();
        orderRepository.Setup(r => r.GetByIdAsync(order.Id)).ReturnsAsync(order);
        serviceRepository.Setup(r => r.GetByIdAsync(service.Id)).ReturnsAsync(service);
        orderRepository.Setup(r => r.GetServiceOrderServiceById(order.Id, service.Id)).ReturnsAsync(existingAttachment);

        var useCase = new AttachServiceUseCase(orderRepository.Object, serviceRepository.Object, orderServiceRepository.Object);
        var result = await useCase.ExecuteAsync(new AttachServiceRequest(order.Id, service.Id, 59.99m));

        result.success.Should().BeFalse();
        result.error.Should().Be("Service already attached to the service order.");
        orderServiceRepository.Verify(r => r.AddAsync(It.IsAny<ServiceOrderService>()), Times.Never);
    }
}
