using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Notifications;
using TC1.RepairShop.Application.Quotes.EventHandlers;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Events;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.EventHandlers;

public class QuoteCreatedEventHandlerTests
{

    [Fact]
    public async Task Handle_ShouldSendToCustomerAndUpdate_WhenQuoteExists()
    {
        var user = User.Create("johndoe", "password", "12345678909", "john@example.com", UserRole.Customer, "123456789");
        var order = ServiceOrder.Create(user.Id, Guid.NewGuid());
        // attach user instance to order so handler can access Email/Username
        typeof(ServiceOrder).GetProperty("User")!.SetValue(order, user);
        var quote = Quote.Create(order.Id, 500m);
        Mock<IQuoteRepository> quoteRepository = new Mock<IQuoteRepository>();
        Mock<IServiceOrderRepository> serviceOrderRepository = new Mock<IServiceOrderRepository>();
        Mock<IEmailSender> mailSender = new Mock<IEmailSender>();
        quoteRepository.Setup(r => r.GetByIdAsync(quote.Id)).ReturnsAsync(quote);
        serviceOrderRepository.Setup(r => r.GetByIdDetailedAsync(order.Id)).ReturnsAsync(order);

        var handler = new QuoteCreatedUpdatedEventHandler(quoteRepository.Object, serviceOrderRepository.Object, mailSender.Object);
        var domainEvent = new QuoteCreatedUpdatedEvent(quote.Id);

        await handler.Handle(domainEvent);

        quote.QuoteStatusValue.Should().Be(QuoteStatus.SentToCustomer);
        quoteRepository.Verify(r => r.UpdateAsync(quote), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotUpdate_WhenQuoteDoesNotExist()
    {
        Mock<IQuoteRepository> quoteRepository = new Mock<IQuoteRepository>();
        Mock<IServiceOrderRepository> serviceOrderRepository = new Mock<IServiceOrderRepository>();
        Mock<IEmailSender> mailSender = new Mock<IEmailSender>();
        quoteRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Quote?)null);

        var handler = new QuoteCreatedUpdatedEventHandler(quoteRepository.Object, serviceOrderRepository.Object, mailSender.Object);
        var domainEvent = new QuoteCreatedUpdatedEvent(Guid.NewGuid());

        await handler.Handle(domainEvent);

        quoteRepository.Verify(r => r.UpdateAsync(It.IsAny<Quote>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldNotThrow_WhenRepositoryThrows()
    {
        Mock<IQuoteRepository> quoteRepository = new Mock<IQuoteRepository>();
        Mock<IServiceOrderRepository> serviceOrderRepository = new Mock<IServiceOrderRepository>();
        Mock<IEmailSender> mailSender = new Mock<IEmailSender>();
        quoteRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("db unavailable"));

        var handler = new QuoteCreatedUpdatedEventHandler(quoteRepository.Object, serviceOrderRepository.Object, mailSender.Object);
        var domainEvent = new QuoteCreatedUpdatedEvent(Guid.NewGuid());

        var act = async () => await handler.Handle(domainEvent);

        await act.Should().NotThrowAsync();
    }
}
