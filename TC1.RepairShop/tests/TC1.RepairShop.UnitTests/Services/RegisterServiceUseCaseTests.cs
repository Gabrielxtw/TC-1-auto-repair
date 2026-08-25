using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Services.UseCases;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Services;

public class RegisterServiceUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenNameIsNotTaken()
    {
        var repository = new Mock<IServiceRepository>();
        repository.Setup(r => r.ExistsByNameAsync("Oil Change")).ReturnsAsync(false);

        var useCase = new CreateServiceUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new CreateServiceRequest("Oil Change", "Change engine oil", 59.99m));

        result.success.Should().BeTrue();
        repository.Verify(r => r.AddAsync(It.IsAny<Service>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenNameAlreadyExists()
    {
        var repository = new Mock<IServiceRepository>();
        repository.Setup(r => r.ExistsByNameAsync("Oil Change")).ReturnsAsync(true);

        var useCase = new CreateServiceUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new CreateServiceRequest("Oil Change", "Change engine oil", 59.99m));

        result.success.Should().BeFalse();
        result.error.Should().Be("Serviço já está cadastrado no sistema.");
        repository.Verify(r => r.AddAsync(It.IsAny<Service>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenRepositoryThrowsUnexpectedException()
    {
        var repository = new Mock<IServiceRepository>();
        repository.Setup(r => r.ExistsByNameAsync(It.IsAny<string>())).ThrowsAsync(new Exception("db unavailable"));

        var useCase = new CreateServiceUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new CreateServiceRequest("Oil Change", "Change engine oil", 59.99m));

        result.success.Should().BeFalse();
        result.error.Should().BeEmpty();
    }
}
