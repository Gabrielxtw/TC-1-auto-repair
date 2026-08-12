using System;
using TC1.RepairShop.Domain.Parts;
using TC1.RepairShop.Domain.Common;
using Xunit;

namespace TC1.RepairShop.UnitTests.Parts;

public class PartTests
{
    [Fact]
    public void Create_ShouldInitializePart()
    {
        var part = Part.Create("Brake Pad", 19.99m, 5);

        Assert.NotEqual(Guid.Empty, part.Id);
        Assert.Equal("Brake Pad", part.Name);
        Assert.Equal(19.99m, part.UnitPrice);
        Assert.Equal(0, part.StockQuantity);
        Assert.Equal(5, part.MinimumQuantity);
        Assert.Equal(Status.Active, part.Status);
    }

    [Fact]
    public void ReceiveStock_ShouldIncreaseStockQuantity()
    {
        var part = Part.Create("Oil Filter", 9.5m, 2);

        part.ReceiveStock(10);

        Assert.Equal(10, part.StockQuantity);
    }

    [Fact]
    public void ConsumeStock_ShouldDecreaseStockQuantity()
    {
        var part = Part.Create("Air Filter", 15.0m, 1);

        part.ReceiveStock(5);
        part.ConsumeStock(2);

        Assert.Equal(3, part.StockQuantity);
    }

    [Fact]
    public void Delete_ShouldSetStatusDeleted()
    {
        var part = Part.Create("Spark Plug", 4.5m, 10);

        part.Delete();

        Assert.Equal(Status.Deleted, part.Status);
    }
}
