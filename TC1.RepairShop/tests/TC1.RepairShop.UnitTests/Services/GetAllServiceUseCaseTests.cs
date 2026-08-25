using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Services.UseCases;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Services;

public class GetAllServiceUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnMappedServices_WhenRepositorySucceeds()
    {
        var service = Service.Create("Oil Change", "Change engine oil", 59.99m);
        var repository = new Mock<IServiceRepository>();
        repository.Setup(r => r.GetAllAsync()).ReturnsAsync(new[] { service });

        var useCase = new GetAllServiceUseCase(repository.Object);
        var result = await useCase.ExecuteAsync();

        result.success.Should().BeTrue();
        result.data.Services.Should().ContainSingle(s => s.Name == "Oil Change");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnEmptyFailure_WhenRepositoryThrows()
    {
        var repository = new Mock<IServiceRepository>();
        repository.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("db unavailable"));

        var useCase = new GetAllServiceUseCase(repository.Object);
        var result = await useCase.ExecuteAsync();

        result.success.Should().BeFalse();
        result.data.Services.Should().BeEmpty();
    }
}
