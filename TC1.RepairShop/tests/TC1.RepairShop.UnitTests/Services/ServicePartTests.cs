using System;
using TC1.RepairShop.Domain.Entities.Services;
using Xunit;

namespace TC1.RepairShop.UnitTests.Services;

public class ServicePartTests
{
    [Fact]
    public void Create_ShouldInitializeServicePart()
    {
        var serviceId = Guid.NewGuid();
        var partId = Guid.NewGuid();
        var sp = ServicePart.Create(serviceId, partId, 4);

        Assert.NotEqual(Guid.Empty, sp.Id);
        Assert.Equal(serviceId, sp.ServiceId);
        Assert.Equal(partId, sp.PartId);
        Assert.Equal(4, sp.Quantity);
    }
}
