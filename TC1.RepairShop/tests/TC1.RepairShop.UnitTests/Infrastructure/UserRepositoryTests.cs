using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Infrastructure.Data;
using TC1.RepairShop.Infrastructure.Data.Repositories;
using Xunit;

namespace TC1.RepairShop.UnitTests.Infrastructure;

public class UserRepositoryTests
{
    private static RepairShopDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<RepairShopDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new RepairShopDbContext(options, Mock.Of<IPublisher>());
    }

    private static User CreateUser(string username = "mechanic1")
    {
        return User.Create(username, "Password@123", "01098843371", "mechanic1@example.com", UserRole.Staff, "11999999999");
    }

    [Fact]
    public async Task AddAsync_ThenGetByIdAsync_ShouldReturnUser()
    {
        await using var context = CreateContext();
        var repository = new UserRepository(context);
        var user = CreateUser();

        await repository.AddAsync(user);

        var stored = await repository.GetByIdAsync(user.Id);

        Assert.NotNull(stored);
        Assert.Equal(user.Id, stored!.Id);
        Assert.Equal(user.Username, stored.Username);
    }

    [Fact]
    public async Task GetByIdAsync_WithUnknownId_ShouldReturnNull()
    {
        await using var context = CreateContext();
        var repository = new UserRepository(context);

        var stored = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(stored);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        await using var context = CreateContext();
        var repository = new UserRepository(context);
        var user1 = CreateUser("mechanic1");
        var user2 = CreateUser("mechanic2");
        context.Users.Add(user1);
        context.Users.Add(user2);
        await context.SaveChangesAsync();

        var all = await repository.GetAllAsync();

        Assert.Equal(2, all.Count());
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        await using var context = CreateContext();
        var repository = new UserRepository(context);
        var user = CreateUser();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        user.UpdateProfile("updatedUsername", UserRole.Admin);
        await repository.UpdateAsync(user);

        var stored = await repository.GetByIdAsync(user.Id);
        Assert.NotNull(stored);
        Assert.Equal("updatedUsername", stored!.Username);
        Assert.Equal(UserRole.Admin, stored.Role);
    }

    [Fact]
    public async Task DeleteAsync_WithUnknownId_ShouldNotThrow()
    {
        await using var context = CreateContext();
        var repository = new UserRepository(context);

        await repository.DeleteAsync(Guid.NewGuid());

        Assert.True(true);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrueForStoredEntity()
    {
        await using var context = CreateContext();
        var repository = new UserRepository(context);
        var user = CreateUser();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var exists = await repository.ExistsAsync(user.Id);
        var notExists = await repository.ExistsAsync(Guid.NewGuid());

        Assert.True(exists);
        Assert.False(notExists);
    }

    [Fact]
    public async Task GetByUsernameAsync_ShouldReturnMatchingUser()
    {
        await using var context = CreateContext();
        var repository = new UserRepository(context);
        var user = CreateUser("mechanic1");
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var result = await repository.GetByUsernameAsync("mechanic1");

        Assert.NotNull(result);
        Assert.Equal(user.Id, result!.Id);
    }

    [Fact]
    public async Task GetByUsernameAsync_WithUnknownUsername_ShouldReturnNull()
    {
        await using var context = CreateContext();
        var repository = new UserRepository(context);
        var user = CreateUser("mechanic1");
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var result = await repository.GetByUsernameAsync("unknownUser");

        Assert.Null(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetByUsernameAsync_WithNullOrWhitespaceUsername_ShouldReturnNull(string? username)
    {
        await using var context = CreateContext();
        var repository = new UserRepository(context);

        var result = await repository.GetByUsernameAsync(username!);

        Assert.Null(result);
    }
}
