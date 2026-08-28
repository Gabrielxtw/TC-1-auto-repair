using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Quotes.UseCases;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Quotes;

public class CreateQuoteUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenServiceOrderExists()
    {
        var order = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());
        var quoteRepository = new Mock<IQuoteRepository>();
        var orderRepository = new Mock<IServiceOrderRepository>();
        orderRepository.Setup(r => r.GetByIdAsync(order.Id)).ReturnsAsync(order);

        var useCase = new CreateQuoteUseCase(quoteRepository.Object, orderRepository.Object);
        var result = await useCase.ExecuteAsync(new CreateQuoteRequest(order.Id, 500m));

        result.success.Should().BeTrue();
        order.QuoteId.Should().Be(result.data.Id);
        quoteRepository.Verify(r => r.AddAsync(It.IsAny<Quote>()), Times.Once);
        orderRepository.Verify(r => r.UpdateAsync(order), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenServiceOrderDoesNotExist()
    {
        var quoteRepository = new Mock<IQuoteRepository>();
        var orderRepository = new Mock<IServiceOrderRepository>();
        orderRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ServiceOrder?)null);

        var useCase = new CreateQuoteUseCase(quoteRepository.Object, orderRepository.Object);
        var result = await useCase.ExecuteAsync(new CreateQuoteRequest(Guid.NewGuid(), 500m));

        result.success.Should().BeFalse();
        result.error.Should().Be("Service order not found.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenRepositoryThrowsUnexpectedException()
    {
        var quoteRepository = new Mock<IQuoteRepository>();
        var orderRepository = new Mock<IServiceOrderRepository>();
        orderRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("db unavailable"));

        var useCase = new CreateQuoteUseCase(quoteRepository.Object, orderRepository.Object);
        var result = await useCase.ExecuteAsync(new CreateQuoteRequest(Guid.NewGuid(), 500m));

        result.success.Should().BeFalse();
        result.error.Should().Be("db unavailable");
    }
}
