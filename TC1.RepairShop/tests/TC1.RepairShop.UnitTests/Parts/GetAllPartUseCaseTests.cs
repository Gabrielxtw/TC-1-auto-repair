using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Parts.UseCases;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Parts;

public class GetAllPartUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnMappedParts_WhenRepositorySucceeds()
    {
        var part = Part.Create("Brake Pad", 19.99m, stockQuantity: 5);
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetAllAsync()).ReturnsAsync(new[] { part });

        var useCase = new GetAllPartUseCase(repository.Object);
        var result = await useCase.ExecuteAsync();

        result.success.Should().BeTrue();
        result.data.Parts.Should().ContainSingle(p => p.Name == "Brake Pad" && p.StockQuantity == 5);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnEmptyFailure_WhenRepositoryThrows()
    {
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("db unavailable"));

        var useCase = new GetAllPartUseCase(repository.Object);
        var result = await useCase.ExecuteAsync();

        result.success.Should().BeFalse();
        result.data.Parts.Should().BeEmpty();
    }
}
