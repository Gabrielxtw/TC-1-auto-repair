using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Parts.UseCases;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Parts;

public class DeactivatePartUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenPartIsActive()
    {
        var part = Part.Create("Brake Pad", 19.99m);
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetByIdAsync(part.Id)).ReturnsAsync(part);

        var useCase = new DeactivatePartUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new DeactivePartRequest(part.Id));

        result.success.Should().BeTrue();
        part.Status.Should().Be(Status.Inactive);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenPartIsAlreadyInactive()
    {
        var part = Part.Create("Brake Pad", 19.99m);
        part.Deactivate();
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetByIdAsync(part.Id)).ReturnsAsync(part);

        var useCase = new DeactivatePartUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new DeactivePartRequest(part.Id));

        result.success.Should().BeFalse();
        result.error.Should().Be("Cannot do action on an inactive entity.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenPartDoesNotExist()
    {
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Part?)null);

        var useCase = new DeactivatePartUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new DeactivePartRequest(Guid.NewGuid()));

        result.success.Should().BeFalse();
        result.error.Should().Be("Not Found");
    }
}
