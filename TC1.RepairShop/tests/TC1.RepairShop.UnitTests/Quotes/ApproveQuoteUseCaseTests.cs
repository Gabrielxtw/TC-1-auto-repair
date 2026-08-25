using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Quotes.UseCases;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Quotes;

public class ApproveQuoteUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenQuoteExists()
    {
        var quote = Quote.Create(Guid.NewGuid(), 500m);
        var repository = new Mock<IQuoteRepository>();
        repository.Setup(r => r.GetByIdAsync(quote.Id)).ReturnsAsync(quote);

        var useCase = new ApproveQuoteUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(quote.Id);

        result.success.Should().BeTrue();
        quote.QuoteStatusValue.Should().Be(QuoteStatus.Approved);
        repository.Verify(r => r.UpdateAsync(quote), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenQuoteDoesNotExist()
    {
        var repository = new Mock<IQuoteRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Quote?)null);

        var useCase = new ApproveQuoteUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(Guid.NewGuid());

        result.success.Should().BeFalse();
        result.error.Should().Be("Quote not found.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenRepositoryThrowsUnexpectedException()
    {
        var repository = new Mock<IQuoteRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("db unavailable"));

        var useCase = new ApproveQuoteUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(Guid.NewGuid());

        result.success.Should().BeFalse();
        result.error.Should().BeEmpty();
    }
}
