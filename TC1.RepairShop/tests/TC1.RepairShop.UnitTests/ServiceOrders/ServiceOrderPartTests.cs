using System;
using TC1.RepairShop.Domain.ServiceOrders;
using Xunit;

namespace TC1.RepairShop.UnitTests.ServiceOrders;

public class ServiceOrderPartTests
{
    [Fact]
    public void Create_ShouldInitializeServiceOrderPart_WithCustomerSupplied_ResetUnitPriceToZero()
    {
        var part = ServiceOrderPart.Create(Guid.NewGuid(), Guid.NewGuid(), 2, 50m, true);

        Assert.NotEqual(Guid.Empty, part.Id);
        Assert.True(part.SuppliedByCustomer);
        Assert.Equal(0m, part.UnitPrice);
    }

    [Fact]
    public void Create_ShouldInitializeServiceOrderPart_WithSupplierPrice()
    {
        var part = ServiceOrderPart.Create(Guid.NewGuid(), Guid.NewGuid(), 3, 30m, false);

        Assert.NotEqual(Guid.Empty, part.Id);
        Assert.False(part.SuppliedByCustomer);
        Assert.Equal(30m, part.UnitPrice);
    }
}
