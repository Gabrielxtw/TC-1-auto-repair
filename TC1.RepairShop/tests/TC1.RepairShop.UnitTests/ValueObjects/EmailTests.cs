using FluentAssertions;
using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.ValueObjects;
using Xunit;

namespace TC1.RepairShop.UnitTests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Create_ShouldNormalizeToLowercase()
    {
        var email = Email.Create("Alice@Example.COM");

        email.Value.Should().Be("alice@example.com");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_ShouldThrow_WhenValueIsEmpty(string? value)
    {
        var act = () => Email.Create(value!);

        act.Should().Throw<BusinessException>()
            .WithMessage("The email value must be a valid email address.");
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("missing-domain@")]
    [InlineData("@missing-local.com")]
    [InlineData("no-at-sign.com")]
    public void Create_ShouldThrow_WhenFormatIsInvalid(string value)
    {
        var act = () => Email.Create(value);

        act.Should().Throw<BusinessException>()
            .WithMessage("The email value must be a valid email address.");
    }
}
