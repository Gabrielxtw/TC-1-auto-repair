using System;
using FluentAssertions;
using Xunit;
using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.UnitTests.Parts;

public class PartTests
{
    [Fact]
    public void Create_ShouldInitializePart()
    {
        var part = Part.Create("Brake Pad", 19.99m);

        Assert.NotEqual(Guid.Empty, part.Id);
        Assert.Equal("Brake Pad", part.Name);
        Assert.Equal(19.99m, part.Price);
        Assert.Equal(Status.Active, part.Status);
    }

    [Fact]
    public void Delete_ShouldSetStatusDeleted()
    {
        var part = Part.Create("Spark Plug", 4.5m);

        part.Delete();

        Assert.Equal(Status.Deleted, part.Status);
    }

    [Fact]
    public void ReceiveStock_ShouldIncreaseQuantity_WhenPartIsActive()
    {
        var part = Part.Create("Brake Pad", 19.99m, stockQuantity: 5);

        part.ReceiveStock(3);

        part.StockQuantity.Should().Be(8);
    }

    [Fact]
    public void ReceiveStock_ShouldThrow_WhenPartIsInactive()
    {
        var part = Part.Create("Brake Pad", 19.99m);
        part.Deactivate();

        var act = () => part.ReceiveStock(3);

        act.Should().Throw<BusinessException>()
            .WithMessage("Cannot alter stock from an inactive part.");
    }

    [Fact]
    public void ConsumeStock_ShouldDecreaseQuantity_WhenPartIsActive()
    {
        var part = Part.Create("Brake Pad", 19.99m, stockQuantity: 5);

        part.ConsumeStock(2);

        part.StockQuantity.Should().Be(3);
    }

    [Fact]
    public void ConsumeStock_ShouldThrow_WhenPartIsInactive()
    {
        var part = Part.Create("Brake Pad", 19.99m, stockQuantity: 5);
        part.Deactivate();

        var act = () => part.ConsumeStock(2);

        act.Should().Throw<BusinessException>()
            .WithMessage("Cannot alter stock from an inactive part.");
    }
}
