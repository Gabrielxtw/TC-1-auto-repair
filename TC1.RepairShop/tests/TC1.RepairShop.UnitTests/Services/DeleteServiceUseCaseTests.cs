using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Services.UseCases;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Services;

public class DeleteServiceUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenServiceExists()
    {
        var service = Service.Create("Oil Change", "Change engine oil", 59.99m);
        var repository = new Mock<IServiceRepository>();
        repository.Setup(r => r.GetByIdAsync(service.Id)).ReturnsAsync(service);

        var useCase = new DeleteServiceUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(service.Id);

        result.success.Should().BeTrue();
        service.Status.Should().Be(Status.Deleted);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenServiceDoesNotExist()
    {
        var repository = new Mock<IServiceRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Service?)null);

        var useCase = new DeleteServiceUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(Guid.NewGuid());

        result.success.Should().BeFalse();
        result.error.Should().Be("Not Found");
    }
}
