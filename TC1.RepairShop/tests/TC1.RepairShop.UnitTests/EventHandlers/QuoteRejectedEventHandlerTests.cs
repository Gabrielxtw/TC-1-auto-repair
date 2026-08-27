using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Notifications;
using TC1.RepairShop.Application.Quotes.EventHandlers;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Events;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.EventHandlers;

public class QuoteRejectedEventHandlerTests
{
    [Fact]
    public async Task Handle_ShouldMarkUnderReviewAndUpdate_WhenQuoteExists()
    {
        var order = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());
        var repository = new Mock<IServiceOrderRepository>();
        Mock<IEmailSender> mailSender = new Mock<IEmailSender>();
        repository.Setup(r => r.GetByIdDetailedAsync(order.Id)).ReturnsAsync(order);

        var handler = new QuoteRejectedEventHandler(repository.Object, mailSender.Object);
        var domainEvent = new QuoteRejectedEvent(order.Id);

        await handler.Handle(domainEvent);

        order.OrderStatusValue.Should().Be(ServiceOrderStatus.Cancelled);
        repository.Verify(r => r.UpdateAsync(order), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotUpdate_WhenQuoteDoesNotExist()
    {
        var repository = new Mock<IServiceOrderRepository>();
        Mock<IEmailSender> mailSender = new Mock<IEmailSender>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ServiceOrder?)null);

        var handler = new QuoteRejectedEventHandler(repository.Object, mailSender.Object);
        var domainEvent = new QuoteRejectedEvent(Guid.NewGuid());

        await handler.Handle(domainEvent);

        repository.Verify(r => r.UpdateAsync(It.IsAny<ServiceOrder>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldNotThrow_WhenRepositoryThrows()
    {
        var repository = new Mock<IServiceOrderRepository>();
        Mock<IEmailSender> mailSender = new Mock<IEmailSender>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("db unavailable"));

        var handler = new QuoteRejectedEventHandler(repository.Object, mailSender.Object);
        var domainEvent = new QuoteRejectedEvent(Guid.NewGuid());

        var act = async () => await handler.Handle(domainEvent);

        await act.Should().NotThrowAsync();
    }
}
