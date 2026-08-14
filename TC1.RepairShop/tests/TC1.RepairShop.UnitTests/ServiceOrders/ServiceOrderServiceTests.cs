using System;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using Xunit;

namespace TC1.RepairShop.UnitTests.ServiceOrders;

public class ServiceOrderServiceTests
{
    [Fact]
    public void Create_ShouldInitializeServiceOrderService()
    {
        var serviceOrderId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();

        var sos = ServiceOrderService.Create(serviceOrderId, serviceId);

        Assert.NotEqual(Guid.Empty, sos.Id);
        Assert.Equal(serviceOrderId, sos.ServiceOrderId);
        Assert.Equal(serviceId, sos.ServiceId);
    }
}
