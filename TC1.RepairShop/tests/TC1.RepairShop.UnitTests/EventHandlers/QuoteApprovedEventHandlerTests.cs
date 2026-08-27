using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Quotes.EventHandlers;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Events;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.EventHandlers;

public class QuoteApprovedEventHandlerTests
{
    private static ServiceOrder CreateAwaitingApprovalOrder()
    {
        var order = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());
        order.AdvanceTo(ServiceOrderStatus.UnderDiagnosis);
        order.AdvanceTo(ServiceOrderStatus.AwaitingApproval);
        return order;
    }

    [Fact]
    public async Task Handle_ShouldAdvanceOrderToInProgress_WhenOrderExists()
    {
        var order = CreateAwaitingApprovalOrder();
        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.GetByIdAsync(order.Id)).ReturnsAsync(order);

        var handler = new QuoteApprovedEventHandler(repository.Object);
        var domainEvent = new QuoteApprovedEvent(order.Id);

        await handler.Handle(domainEvent);

        order.OrderStatusValue.Should().Be(ServiceOrderStatus.InProgress);
        repository.Verify(r => r.UpdateAsync(order), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotUpdate_WhenOrderDoesNotExist()
    {
        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ServiceOrder?)null);

        var handler = new QuoteApprovedEventHandler(repository.Object);
        var domainEvent = new QuoteApprovedEvent(Guid.NewGuid());

        await handler.Handle(domainEvent);

        repository.Verify(r => r.UpdateAsync(It.IsAny<ServiceOrder>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldSwallowException_WhenRepositoryThrowsOnGetById()
    {
        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("db unavailable"));

        var handler = new QuoteApprovedEventHandler(repository.Object);
        var domainEvent = new QuoteApprovedEvent(Guid.NewGuid());

        var act = async () => await handler.Handle(domainEvent);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Handle_ShouldSwallowException_WhenRepositoryThrowsOnUpdate()
    {
        var order = CreateAwaitingApprovalOrder();
        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.GetByIdAsync(order.Id)).ReturnsAsync(order);
        repository.Setup(r => r.UpdateAsync(It.IsAny<ServiceOrder>())).ThrowsAsync(new Exception("db unavailable"));

        var handler = new QuoteApprovedEventHandler(repository.Object);
        var domainEvent = new QuoteApprovedEvent(order.Id);

        var act = async () => await handler.Handle(domainEvent);

        await act.Should().NotThrowAsync();
    }
}
