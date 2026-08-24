using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Parts.UseCases;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Parts;

public class ReceiveStockUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenPartIsActive()
    {
        var part = Part.Create("Brake Pad", 19.99m, stockQuantity: 5);
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetByIdAsync(part.Id)).ReturnsAsync(part);

        var useCase = new ReceiveStockUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new ReceiveStockRequest(part.Id, 3));

        result.success.Should().BeTrue();
        part.StockQuantity.Should().Be(8);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenPartIsInactive()
    {
        var part = Part.Create("Brake Pad", 19.99m);
        part.Deactivate();
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetByIdAsync(part.Id)).ReturnsAsync(part);

        var useCase = new ReceiveStockUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new ReceiveStockRequest(part.Id, 3));

        result.success.Should().BeFalse();
        result.error.Should().Be("Cannot alter stock from an inactive part.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenPartDoesNotExist()
    {
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Part?)null);

        var useCase = new ReceiveStockUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new ReceiveStockRequest(Guid.NewGuid(), 3));

        result.success.Should().BeFalse();
        result.error.Should().Be("Not Found");
    }
}
