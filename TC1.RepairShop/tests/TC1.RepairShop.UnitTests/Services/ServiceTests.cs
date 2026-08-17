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
        var service = Service.Create("Oil Change", "Change engine oil and filter", 59.99m);

        Assert.NotEqual(Guid.Empty, service.Id);
        Assert.Equal("Oil Change", service.Name);
        Assert.Equal("Change engine oil and filter", service.Description);
        Assert.Equal(59.99m, service.Price);
        Assert.Equal(Status.Active, service.Status);
    }

    [Fact]
    public void Delete_ShouldSetStatusDeleted()
    {
        var service = Service.Create("Battery Replacement", "Replace battery", 120m);

        service.Delete();

        Assert.Equal(Status.Deleted, service.Status);
    }
}
