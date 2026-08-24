using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Parts.UseCases;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Parts;

public class GetPartByIdCaseUseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnPartData_WhenFound()
    {
        var part = Part.Create("Brake Pad", 19.99m, stockQuantity: 5);
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetByIdAsync(part.Id)).ReturnsAsync(part);

        var useCase = new GetPartByIdUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(part.Id);

        result.success.Should().BeTrue();
        result.data.Should().NotBeNull();
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenPartDoesNotExist()
    {
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Part?)null);

        var useCase = new GetPartByIdUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(Guid.NewGuid());

        result.success.Should().BeFalse();
    }
}
