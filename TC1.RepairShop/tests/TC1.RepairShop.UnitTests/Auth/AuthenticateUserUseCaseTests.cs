using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Auth;
using TC1.RepairShop.Application.Auth.UseCases;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Auth;

public class AuthenticateUserUseCaseTests
{
    private static User CreateUser() =>
        User.Create("alice", "Passw0rd!", "52998224725", "alice@example.com", UserRole.Staff, "1999999999");

    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenCredentialsAreValid()
    {
        var user = CreateUser();
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(r => r.GetByUsernameAsync("alice")).ReturnsAsync(user);
        var tokenService = new Mock<ITokenService>();
        tokenService.Setup(t => t.GenerateStaffToken(user)).Returns("fake-token");

        var useCase = new AuthenticateUserUseCase(userRepository.Object, tokenService.Object);
        var result = await useCase.ExecuteAsync(new AuthenticateUserRequest("alice", "Passw0rd!"));

        result.Success.Should().BeTrue();
        result.Token.Should().Be("fake-token");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenUsernameDoesNotExist()
    {
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(r => r.GetByUsernameAsync("alice")).ReturnsAsync((User?)null);
        var tokenService = new Mock<ITokenService>();

        var useCase = new AuthenticateUserUseCase(userRepository.Object, tokenService.Object);
        var result = await useCase.ExecuteAsync(new AuthenticateUserRequest("alice", "Passw0rd!"));

        result.Success.Should().BeFalse();
        result.Token.Should().BeNull();
        tokenService.Verify(t => t.GenerateStaffToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenPasswordIsIncorrect()
    {
        var user = CreateUser();
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(r => r.GetByUsernameAsync("alice")).ReturnsAsync(user);
        var tokenService = new Mock<ITokenService>();

        var useCase = new AuthenticateUserUseCase(userRepository.Object, tokenService.Object);
        var result = await useCase.ExecuteAsync(new AuthenticateUserRequest("alice", "WrongPassword"));

        result.Success.Should().BeFalse();
        result.Token.Should().BeNull();
        tokenService.Verify(t => t.GenerateStaffToken(It.IsAny<User>()), Times.Never);
    }
}
