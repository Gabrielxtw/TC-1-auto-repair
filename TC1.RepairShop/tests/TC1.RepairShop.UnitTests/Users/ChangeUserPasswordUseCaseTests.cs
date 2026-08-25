using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Users.UseCases;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Users;

public class ChangeUserPasswordUseCaseTests
{
    private const string ValidDocument = "52998224725";

    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenUserExists()
    {
        var user = User.Create("alice", "OldPass1!", ValidDocument, "alice@example.com", UserRole.Staff, "1999999999");
        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var useCase = new ChangeUserPasswordUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new ChangeUserPasswordRequest(user.Id, "NewPass2!"));

        result.success.Should().BeTrue();
        user.VerifyPassword("NewPass2!").Should().BeTrue();
        repository.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenUserDoesNotExist()
    {
        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var useCase = new ChangeUserPasswordUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(new ChangeUserPasswordRequest(Guid.NewGuid(), "NewPass2!"));

        result.success.Should().BeFalse();
        result.error.Should().Be("User not found.");
    }
}
