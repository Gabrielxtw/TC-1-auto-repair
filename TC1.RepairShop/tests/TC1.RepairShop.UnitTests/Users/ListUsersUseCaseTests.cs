using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Users.UseCases;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Users;

public class ListUsersUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnAllUsers_FromRepository()
    {
        var user = User.Create("alice", "Passw0rd!", "52998224725", "alice@example.com", UserRole.Staff, "1999999999");
        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetAllAsync()).ReturnsAsync(new[] { user });

        var useCase = new ListUsersUseCase(repository.Object);
        var result = await useCase.ExecuteAsync();

        result.data.Users.Should().ContainSingle().Which.Username.Should().Be("alice");
    }
}
