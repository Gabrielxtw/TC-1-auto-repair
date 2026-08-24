using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Parts.UseCases;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Parts;

public class ConsumeStockUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenPartIsActive()
    {
        var part = Part.Create("Brake Pad", 19.99m, stockQuantity: 5);
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetByIdAsync(part.Id)).ReturnsAsync(part);

        var useCase = new ConsumeStockUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new ConsumeStockRequest(part.Id, 3));

        result.success.Should().BeTrue();
        part.StockQuantity.Should().Be(2);
        repository.Verify(r => r.UpdateAsync(part), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenPartIsInactive()
    {
        var part = Part.Create("Brake Pad", 19.99m, stockQuantity: 5);
        part.Deactivate();
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetByIdAsync(part.Id)).ReturnsAsync(part);

        var useCase = new ConsumeStockUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new ConsumeStockRequest(part.Id, 3));

        result.success.Should().BeFalse();
        result.error.Should().Be("Cannot alter stock from an inactive part.");
        part.StockQuantity.Should().Be(5);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenPartDoesNotExist()
    {
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Part?)null);

        var useCase = new ConsumeStockUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new ConsumeStockRequest(Guid.NewGuid(), 3));

        result.success.Should().BeFalse();
        result.error.Should().Be("Not Found");
    }
}
