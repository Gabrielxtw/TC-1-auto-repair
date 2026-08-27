using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.ServiceOrders.EventHandlers;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Events;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.EventHandlers;

public class DiagnosisConcludedEventHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdatePriceAndReturnEarly_WhenQuoteIdIsSetAndQuoteExists()
    {
        var quote = Quote.Create(Guid.NewGuid(), 100m);
        var quoteRepository = new Mock<IQuoteRepository>();
        var serviceOrderRepository = new Mock<IServiceOrderRepository>();
        quoteRepository.Setup(r => r.GetByIdAsync(quote.Id)).ReturnsAsync(quote);

        var handler = new DiagnosisConcludedEventHandler(quoteRepository.Object, serviceOrderRepository.Object);
        var domainEvent = new DiagnosisConcludedEvent(Guid.NewGuid(), 250m, quote.Id);

        await handler.Handle(domainEvent);

        quote.Price.Should().Be(250m);
        quoteRepository.Verify(r => r.UpdateAsync(quote), Times.Once);
        quoteRepository.Verify(r => r.AddAsync(It.IsAny<Quote>()), Times.Never);
        serviceOrderRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        serviceOrderRepository.Verify(r => r.UpdateAsync(It.IsAny<ServiceOrder>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldSwallowException_WhenQuoteIdIsSetButQuoteNotFound()
    {
        var quoteRepository = new Mock<IQuoteRepository>();
        var serviceOrderRepository = new Mock<IServiceOrderRepository>();
        quoteRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Quote?)null);

        var handler = new DiagnosisConcludedEventHandler(quoteRepository.Object, serviceOrderRepository.Object);
        var domainEvent = new DiagnosisConcludedEvent(Guid.NewGuid(), 250m, Guid.NewGuid());

        var act = async () => await handler.Handle(domainEvent);

        await act.Should().NotThrowAsync();
        quoteRepository.Verify(r => r.UpdateAsync(It.IsAny<Quote>()), Times.Never);
        quoteRepository.Verify(r => r.AddAsync(It.IsAny<Quote>()), Times.Never);
        serviceOrderRepository.Verify(r => r.UpdateAsync(It.IsAny<ServiceOrder>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCreateQuoteAndAttachToOrder_WhenQuoteIdIsNullAndOrderExists()
    {
        var serviceOrderId = Guid.NewGuid();
        var serviceOrder = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());
        var quoteRepository = new Mock<IQuoteRepository>();
        var serviceOrderRepository = new Mock<IServiceOrderRepository>();
        serviceOrderRepository.Setup(r => r.GetByIdAsync(serviceOrderId)).ReturnsAsync(serviceOrder);

        var handler = new DiagnosisConcludedEventHandler(quoteRepository.Object, serviceOrderRepository.Object);
        var domainEvent = new DiagnosisConcludedEvent(serviceOrderId, 300m, null);

        await handler.Handle(domainEvent);

        quoteRepository.Verify(r => r.AddAsync(It.Is<Quote>(q =>
            q.ServiceOrderId == serviceOrderId && q.Price == 300m)), Times.Once);
        quoteRepository.Verify(r => r.UpdateAsync(It.IsAny<Quote>()), Times.Never);
        serviceOrder.QuoteId.Should().NotBeNull();
        serviceOrderRepository.Verify(r => r.UpdateAsync(serviceOrder), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCreateQuoteButNotUpdateOrder_WhenQuoteIdIsNullAndOrderNotFound()
    {
        var serviceOrderId = Guid.NewGuid();
        var quoteRepository = new Mock<IQuoteRepository>();
        var serviceOrderRepository = new Mock<IServiceOrderRepository>();
        serviceOrderRepository.Setup(r => r.GetByIdAsync(serviceOrderId)).ReturnsAsync((ServiceOrder?)null);

        var handler = new DiagnosisConcludedEventHandler(quoteRepository.Object, serviceOrderRepository.Object);
        var domainEvent = new DiagnosisConcludedEvent(serviceOrderId, 300m, null);

        await handler.Handle(domainEvent);

        quoteRepository.Verify(r => r.AddAsync(It.Is<Quote>(q =>
            q.ServiceOrderId == serviceOrderId && q.Price == 300m)), Times.Once);
        serviceOrderRepository.Verify(r => r.UpdateAsync(It.IsAny<ServiceOrder>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldSwallowException_WhenRepositoryThrows()
    {
        var quote = Quote.Create(Guid.NewGuid(), 100m);
        var quoteRepository = new Mock<IQuoteRepository>();
        var serviceOrderRepository = new Mock<IServiceOrderRepository>();
        quoteRepository.Setup(r => r.GetByIdAsync(quote.Id)).ReturnsAsync(quote);
        quoteRepository.Setup(r => r.UpdateAsync(quote)).ThrowsAsync(new Exception("db unavailable"));

        var handler = new DiagnosisConcludedEventHandler(quoteRepository.Object, serviceOrderRepository.Object);
        var domainEvent = new DiagnosisConcludedEvent(Guid.NewGuid(), 250m, quote.Id);

        var act = async () => await handler.Handle(domainEvent);

        await act.Should().NotThrowAsync();
    }
}
