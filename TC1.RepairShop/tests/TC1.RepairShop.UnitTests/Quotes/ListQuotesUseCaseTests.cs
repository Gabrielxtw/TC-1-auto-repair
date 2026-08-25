using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Quotes.UseCases.ListQuotes;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Quotes;

public class ListQuotesUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnAllQuotes_FromRepository()
    {
        var quote = Quote.Create(Guid.NewGuid(), 500m);
        var repository = new Mock<IQuoteRepository>();
        repository.Setup(r => r.GetAllAsync()).ReturnsAsync(new[] { quote });

        var useCase = new ListQuotesUseCase(repository.Object);
        var result = await useCase.ExecuteAsync();

        result.success.Should().BeTrue();
        result.data.Quotes.Should().ContainSingle().Which.Should().Be(quote);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnEmptyFailure_WhenRepositoryThrows()
    {
        var repository = new Mock<IQuoteRepository>();
        repository.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("db unavailable"));

        var useCase = new ListQuotesUseCase(repository.Object);
        var result = await useCase.ExecuteAsync();

        result.success.Should().BeFalse();
        result.data.Quotes.Should().BeEmpty();
        result.error.Should().Be("db unavailable");
    }
}
