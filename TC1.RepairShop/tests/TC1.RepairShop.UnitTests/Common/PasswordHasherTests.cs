using TC1.RepairShop.Domain.Entities.Common;
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

    [Theory]
    [InlineData("not-a-valid-hash")]
    [InlineData("only.two-parts")]
    [InlineData("notanumber.c2FsdA==.aGFzaA==")]
    public void Verify_WithMalformedHash_ShouldReturnFalse(string malformedHash)
    {
        Assert.False(PasswordHasher.Verify("Passw0rd!", malformedHash));
    }
}
