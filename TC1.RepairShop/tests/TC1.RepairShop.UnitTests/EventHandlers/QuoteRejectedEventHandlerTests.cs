using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Quotes.EventHandlers;
using TC1.RepairShop.Domain.Entities.Quotes;
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
        var quote = Quote.Create(Guid.NewGuid(), 500m);
        var repository = new Mock<IQuoteRepository>();
        repository.Setup(r => r.GetByIdAsync(quote.Id)).ReturnsAsync(quote);

        var handler = new QuoteRejectedEventHandler(repository.Object);
        var domainEvent = new QuoteRejectedEvent(quote.Id, quote.ServiceOrderId);

        await handler.Handle(domainEvent);

        quote.QuoteStatusValue.Should().Be(QuoteStatus.UnderReview);
        repository.Verify(r => r.UpdateAsync(quote), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotUpdate_WhenQuoteDoesNotExist()
    {
        var repository = new Mock<IQuoteRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Quote?)null);

        var handler = new QuoteRejectedEventHandler(repository.Object);
        var domainEvent = new QuoteRejectedEvent(Guid.NewGuid(), Guid.NewGuid());

        await handler.Handle(domainEvent);

        repository.Verify(r => r.UpdateAsync(It.IsAny<Quote>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldNotThrow_WhenRepositoryThrows()
    {
        var repository = new Mock<IQuoteRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("db unavailable"));

        var handler = new QuoteRejectedEventHandler(repository.Object);
        var domainEvent = new QuoteRejectedEvent(Guid.NewGuid(), Guid.NewGuid());

        var act = async () => await handler.Handle(domainEvent);

        await act.Should().NotThrowAsync();
    }
}
