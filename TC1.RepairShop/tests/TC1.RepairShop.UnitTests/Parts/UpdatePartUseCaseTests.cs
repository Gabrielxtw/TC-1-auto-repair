using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Parts.UseCases;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Parts;

public class UpdatePartUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenPartExists()
    {
        var part = Part.Create("Brake Pad", 19.99m);
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetByIdAsync(part.Id)).ReturnsAsync(part);

        var useCase = new UpdatePartUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new UpdatePartRequest(part.Id, "Brake Pad Premium", 29.99m));

        result.success.Should().BeTrue();
        part.Name.Should().Be("Brake Pad Premium");
        part.Price.Should().Be(29.99m);
        repository.Verify(r => r.UpdateAsync(part), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenPartDoesNotExist()
    {
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Part?)null);

        var useCase = new UpdatePartUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new UpdatePartRequest(Guid.NewGuid(), "Brake Pad Premium", 29.99m));

        result.success.Should().BeFalse();
        result.error.Should().Be("Part not found.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenPartIsInactive()
    {
        var part = Part.Create("Brake Pad", 19.99m);
        part.Deactivate();
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetByIdAsync(part.Id)).ReturnsAsync(part);

        var useCase = new UpdatePartUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new UpdatePartRequest(part.Id, "Brake Pad Premium", 29.99m));

        result.success.Should().BeFalse();
        result.error.Should().Be("Cannot alter stock from an inactive part.");
        repository.Verify(r => r.UpdateAsync(It.IsAny<Part>()), Times.Never);
    }
}
