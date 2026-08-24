using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Parts.UseCases;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Parts;

public class DeletePartUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenPartExists()
    {
        var part = Part.Create("Brake Pad", 19.99m);
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetByIdAsync(part.Id)).ReturnsAsync(part);

        var useCase = new DeletePartUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new DeletePartRequest(part.Id));

        result.success.Should().BeTrue();
        part.Status.Should().Be(Status.Deleted);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenPartDoesNotExist()
    {
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Part?)null);

        var useCase = new DeletePartUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new DeletePartRequest(Guid.NewGuid()));

        result.success.Should().BeFalse();
        result.error.Should().Be("Not Found");
    }
}
