using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Services.UseCases;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Services;

public class GetServiceByIdUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnServiceData_WhenFound()
    {
        var service = Service.Create("Oil Change", "Change engine oil", 59.99m);
        var repository = new Mock<IServiceRepository>();
        repository.Setup(r => r.GetByIdAsync(service.Id)).ReturnsAsync(service);

        var useCase = new GetServiceByIdUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(service.Id);

        result.success.Should().BeTrue();
        result.data.Name.Should().Be("Oil Change");
        result.data.description.Should().Be("Change engine oil");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenServiceDoesNotExist()
    {
        var repository = new Mock<IServiceRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Service?)null);

        var useCase = new GetServiceByIdUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(Guid.NewGuid());

        result.success.Should().BeFalse();
    }
}
