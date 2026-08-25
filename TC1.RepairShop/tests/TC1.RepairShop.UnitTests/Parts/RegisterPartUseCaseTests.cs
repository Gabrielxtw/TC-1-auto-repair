using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Parts.UseCases;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Parts;

public class RegisterPartUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenNameIsNotTaken()
    {
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.ExistsByNameAsync("Brake Pad")).ReturnsAsync(false);

        var useCase = new CreatePartUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new CreatePartRequest("Brake Pad", 19.99m, 5));

        result.success.Should().BeTrue();
        repository.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Parts.Part>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenNameAlreadyExists()
    {
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.ExistsByNameAsync("Brake Pad")).ReturnsAsync(true);

        var useCase = new CreatePartUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new CreatePartRequest("Brake Pad", 19.99m, 5));

        result.success.Should().BeFalse();
        result.error.Should().Be("Peça já está cadastrada no sistema.");
        repository.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Parts.Part>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenRepositoryThrowsUnexpectedException()
    {
        var repository = new Mock<IPartRepository>();
        repository.Setup(r => r.ExistsByNameAsync(It.IsAny<string>())).ThrowsAsync(new Exception("db unavailable"));

        var useCase = new CreatePartUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new CreatePartRequest("Brake Pad", 19.99m, 5));

        result.success.Should().BeFalse();
        result.error.Should().Be("Ocorreu um erro ao criar a peça.");
    }
}
