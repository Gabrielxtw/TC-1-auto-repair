using System;
using TC1.RepairShop.Domain.Entities.Clients;
using TC1.RepairShop.Domain.Entities.Common;
using Xunit;

namespace TC1.RepairShop.UnitTests.Clients;

public class UserTests
{
    [Fact]
    public void Create_ShouldInitializeUser()
    {
        var user = User.Create("alice", "Passw0rd!", "User");

        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("alice", user.Username);
        Assert.Equal("User", user.Role);
        Assert.Equal(Status.Active, user.Status);
        Assert.True(user.VerifyPassword("Passw0rd!"));
        Assert.NotEqual("Passw0rd!", user.PasswordHash);
    }

    [Fact]
    public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
    {
        var user = User.Create("alice", "Passw0rd!", "User");

        Assert.False(user.VerifyPassword("WrongPassword"));
    }

    [Fact]
    public void UpdateProfile_ShouldChangeUsernameAndRole()
    {
        var user = User.Create("alice", "Passw0rd!", "User");

        user.UpdateProfile("bob", "Admin");

        Assert.Equal("bob", user.Username);
        Assert.Equal("Admin", user.Role);
    }

    [Fact]
    public void ChangePassword_ShouldUpdateHashAndVerification()
    {
        var user = User.Create("alice", "OldPass1!", "User");
        var oldHash = user.PasswordHash;

        user.ChangePassword("NewPass2!");

        Assert.NotEqual(oldHash, user.PasswordHash);
        Assert.True(user.VerifyPassword("NewPass2!"));
        Assert.False(user.VerifyPassword("OldPass1!"));
    }

    [Fact]
    public void Delete_ShouldSetStatusDeleted()
    {
        var user = User.Create("alice", "Passw0rd!", "User");

        user.Delete();

        Assert.Equal(Status.Deleted, user.Status);
    }
}
