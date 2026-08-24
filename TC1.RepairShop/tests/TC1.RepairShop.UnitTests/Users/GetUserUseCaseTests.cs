using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Users.UseCases;
using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Users;

public class GetUserUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnUser_WhenFound()
    {
        var user = User.Create("alice", "Passw0rd!", "52998224725", "alice@example.com", UserRole.Staff, "1999999999");
        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var useCase = new GetUserUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(user.Id);

        result.data.Id.Should().Be(user.Id);
        result.data.Username.Should().Be(user.Username);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrow_WhenNotFound()
    {
        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var useCase = new GetUserUseCase(repository.Object);
        var act = () => useCase.ExecuteAsync(Guid.NewGuid());

        await act.Should().ThrowAsync<BusinessException>();
    }
}
