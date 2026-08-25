using TC1.RepairShop.Domain.Vehicles;
using TC1.RepairShop.Domain.CustomExceptions;
using Xunit;

namespace TC1.RepairShop.UnitTests.Vehicles;

public class LicensePlateTests
{
    [Theory]
    [InlineData("ABC1234")]
    [InlineData("abc-1234")]
    [InlineData("ABC1D23")]
    [InlineData("abc1d23")]
    public void IsValid_ShouldReturnTrue_ForValidPlates(string plate)
    {
        Assert.True(LicensePlate.IsValid(plate));
    }

    [Theory]
    [InlineData("AB1234")]
    [InlineData("ABCD123")]
    [InlineData("1234ABC")]
    [InlineData("")]
    [InlineData(null)]
    public void IsValid_ShouldReturnFalse_ForInvalidPlates(string? plate)
    {
        Assert.False(LicensePlate.IsValid(plate));
    }

    [Fact]
    public void Create_ShouldNormalizeToUppercaseWithoutSeparators()
    {
        var plate = LicensePlate.Create("abc-1234");

        Assert.Equal("ABC1234", plate.Value);
    }

    [Fact]
    public void Create_ShouldThrow_WhenPlateIsInvalid()
    {
        Assert.Throws<BusinessException>(() => LicensePlate.Create("XYZ"));
    }
}
