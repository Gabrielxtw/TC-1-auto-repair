using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Users.UseCases;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Users;

public class UpdateUserUseCaseTests
{
    private const string ValidDocument = "52998224725";

    private static User CreateUser(string username) =>
        User.Create(username, "Passw0rd!", ValidDocument, $"{username}@example.com", UserRole.Staff, "1999999999");

    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenUserExistsAndUsernameIsAvailable()
    {
        var user = CreateUser("alice");
        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        repository.Setup(r => r.GetByUsernameAsync("bob")).ReturnsAsync((User?)null);

        var useCase = new UpdateUserUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new UpdateUserRequest(user.Id, "bob", UserRole.Admin));

        result.success.Should().BeTrue();
        user.Username.Should().Be("bob");
        user.Role.Should().Be(UserRole.Admin);
        repository.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenUserDoesNotExist()
    {
        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var useCase = new UpdateUserUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new UpdateUserRequest(Guid.NewGuid(), "bob", UserRole.Admin));

        result.success.Should().BeFalse();
        result.error.Should().Be("User not found.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenNewUsernameBelongsToAnotherUser()
    {
        var user = CreateUser("alice");
        var otherUser = CreateUser("bob");
        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        repository.Setup(r => r.GetByUsernameAsync("bob")).ReturnsAsync(otherUser);

        var useCase = new UpdateUserUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new UpdateUserRequest(user.Id, "bob", UserRole.Admin));

        result.success.Should().BeFalse();
        result.error.Should().Be("Username is already taken.");
    }
}
