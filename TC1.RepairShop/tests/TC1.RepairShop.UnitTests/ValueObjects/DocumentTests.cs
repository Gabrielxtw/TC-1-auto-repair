using FluentAssertions;
using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.ValueObjects;
using Xunit;

namespace TC1.RepairShop.UnitTests.ValueObjects;

public class DocumentTests
{
    [Theory]
    [InlineData("52998224725")]
    [InlineData("529.982.247-25")]
    [InlineData("11222333000181")]
    [InlineData("11.222.333/0001-81")]
    public void Create_ShouldSucceed_ForValidCpfOrCnpj(string value)
    {
        var document = Document.Create(value);

        document.Value.Should().Be(value.Trim());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_ShouldThrow_WhenValueIsEmpty(string? value)
    {
        var act = () => Document.Create(value!);

        act.Should().Throw<BusinessException>()
            .WithMessage("The document value must be a valid CPF or CNPJ.");
    }

    [Theory]
    [InlineData("123456789")]
    [InlineData("abc")]
    [InlineData("1234567890123456")]
    public void Create_ShouldThrow_WhenFormatIsInvalid(string value)
    {
        var act = () => Document.Create(value);

        act.Should().Throw<BusinessException>()
            .WithMessage("The document value must be a valid CPF or CNPJ.");
    }
}
