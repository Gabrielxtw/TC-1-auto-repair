using TC1.RepairShop.Domain.Common;
using Xunit;

namespace TC1.RepairShop.UnitTests.Common;

public class PasswordHasherTests
{
    [Fact]
    public void Verify_WithCorrectPassword_ShouldReturnTrue()
    {
        var hash = PasswordHasher.Hash("Passw0rd!");

        Assert.True(PasswordHasher.Verify("Passw0rd!", hash));
    }

    [Fact]
    public void Verify_WithWrongPassword_ShouldReturnFalse()
    {
        var hash = PasswordHasher.Hash("Passw0rd!");

        Assert.False(PasswordHasher.Verify("WrongPassword!", hash));
    }

    [Fact]
    public void Hash_ShouldProduceDifferentOutputForSamePassword()
    {
        var hash1 = PasswordHasher.Hash("Passw0rd!");
        var hash2 = PasswordHasher.Hash("Passw0rd!");

        Assert.NotEqual(hash1, hash2);
    }
}
