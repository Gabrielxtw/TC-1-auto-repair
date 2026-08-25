using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Services.UseCases;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Services;

public class DeactiveServiceUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenServiceIsActive()
    {
        var service = Service.Create("Oil Change", "Change engine oil", 59.99m);
        var repository = new Mock<IServiceRepository>();
        repository.Setup(r => r.GetByIdAsync(service.Id)).ReturnsAsync(service);

        var useCase = new DeactiveServiceUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new DeactiveServiceRequest(service.Id));

        result.success.Should().BeTrue();
        service.Status.Should().Be(Status.Inactive);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenServiceIsAlreadyInactive()
    {
        var service = Service.Create("Oil Change", "Change engine oil", 59.99m);
        service.Deactivate();
        var repository = new Mock<IServiceRepository>();
        repository.Setup(r => r.GetByIdAsync(service.Id)).ReturnsAsync(service);

        var useCase = new DeactiveServiceUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new DeactiveServiceRequest(service.Id));

        result.success.Should().BeFalse();
        result.error.Should().Be("Cannot do action on an inactive entity.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenServiceDoesNotExist()
    {
        var repository = new Mock<IServiceRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Service?)null);

        var useCase = new DeactiveServiceUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new DeactiveServiceRequest(Guid.NewGuid()));

        result.success.Should().BeFalse();
        result.error.Should().Be("Not Found");
    }
}
