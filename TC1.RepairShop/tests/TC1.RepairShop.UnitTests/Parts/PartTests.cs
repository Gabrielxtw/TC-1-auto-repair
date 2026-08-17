using System;
using Xunit;
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
}
