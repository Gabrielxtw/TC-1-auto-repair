using TC1.RepairShop.Domain.Registration;
using Xunit;

namespace TC1.RepairShop.UnitTests.Registration;

public class NationalIdTests
{
    [Theory]
    [InlineData("529.982.247-25")]
    [InlineData("52998224725")]
    [InlineData("11.222.333/0001-81")]
    [InlineData("11222333000181")]
    public void IsValid_ShouldReturnTrue_ForValidDocuments(string document)
    {
        Assert.True(NationalId.IsValid(document));
    }

    [Theory]
    [InlineData("111.111.111-11")]
    [InlineData("12345678900")]
    [InlineData("11.111.111/1111-11")]
    [InlineData("123")]
    [InlineData("")]
    [InlineData(null)]
    public void IsValid_ShouldReturnFalse_ForInvalidDocuments(string? document)
    {
        Assert.False(NationalId.IsValid(document));
    }

    [Fact]
    public void Create_ShouldThrow_WhenDocumentIsInvalid()
    {
        Assert.Throws<ArgumentException>(() => NationalId.Create("123"));
    }

    [Fact]
    public void Create_ShouldReturnDigitsOnly_WhenDocumentIsValid()
    {
        var nationalId = NationalId.Create("529.982.247-25");

        Assert.Equal("52998224725", nationalId.Value);
    }
}
