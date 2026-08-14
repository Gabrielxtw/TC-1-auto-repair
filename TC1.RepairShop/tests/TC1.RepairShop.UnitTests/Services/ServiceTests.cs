using System;
using Xunit;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.UnitTests.Services;

public class ServiceTests
{
    [Fact]
    public void Create_ShouldInitializeService()
    {
        var service = Service.Create("Oil Change", "Change engine oil and filter");

        Assert.NotEqual(Guid.Empty, service.Id);
        Assert.Equal("Oil Change", service.Name);
        Assert.Equal("Change engine oil and filter", service.Description);
        Assert.Equal(Status.Active, service.Status);
    }

    [Fact]
    public void AddPart_ShouldAddServicePart()
    {
        var service = Service.Create("Wheel Alignment", "Align wheels");
        var partId = Guid.NewGuid();

        service.AddPart(partId, 2);

        Assert.Contains(service.Parts, p => p.PartId == partId && p.Quantity == 2);
    }

    [Fact]
    public void Delete_ShouldSetStatusDeleted()
    {
        var service = Service.Create("Battery Replacement", "Replace battery");

        service.Delete();

        Assert.Equal(Status.Deleted, service.Status);
    }
}
