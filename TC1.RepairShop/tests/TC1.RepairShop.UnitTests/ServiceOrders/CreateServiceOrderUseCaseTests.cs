using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.ServiceOrders.UseCases;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.ServiceOrders;

public class CreateServiceOrderUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenRepositorySucceeds()
    {
        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>())).Returns(Task.CompletedTask);

        var useCase = new CreateServiceOrderUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new CreateServiceOrderRequest(Guid.NewGuid(), Guid.NewGuid()));

        result.success.Should().BeTrue();
        result.data.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenRepositoryThrowsUnexpectedException()
    {
        var repository = new Mock<IServiceOrderRepository>();
        repository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>())).ThrowsAsync(new Exception("db unavailable"));

        var useCase = new CreateServiceOrderUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new CreateServiceOrderRequest(Guid.NewGuid(), Guid.NewGuid()));

        result.success.Should().BeFalse();
        result.error.Should().Be("db unavailable");
    }
}
