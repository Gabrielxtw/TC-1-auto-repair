using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Users.UseCases;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Users;

public class CreateUserUseCaseTests
{
    private const string ValidDocument = "52998224725";

    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenUsernameIsAvailable()
    {
        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetByUsernameAsync("alice")).ReturnsAsync((User?)null);

        var useCase = new CreateUserUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(
            new CreateUserRequest("alice", "Passw0rd!", ValidDocument, "alice@example.com", UserRole.Staff, "1999999999"));

        result.success.Should().BeTrue();
        result.data.Should().NotBeNull();
        repository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenUsernameIsAlreadyTaken()
    {
        var existingUser = User.Create("alice", "Passw0rd!", ValidDocument, "alice@example.com", UserRole.Staff, "1999999999");
        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetByUsernameAsync("alice")).ReturnsAsync(existingUser);

        var useCase = new CreateUserUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(
            new CreateUserRequest("alice", "Passw0rd!", ValidDocument, "alice@example.com", UserRole.Staff, "1999999999"));

        result.success.Should().BeFalse();
        result.error.Should().Be("Username is already taken.");
        repository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenDocumentIsInvalid()
    {
        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetByUsernameAsync("alice")).ReturnsAsync((User?)null);

        var useCase = new CreateUserUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(
            new CreateUserRequest("alice", "Passw0rd!", "invalid-document", "alice@example.com", UserRole.Staff, "1999999999"));

        result.success.Should().BeFalse();
        result.error.Should().Be("The document value must be a valid CPF or CNPJ.");
    }
}
