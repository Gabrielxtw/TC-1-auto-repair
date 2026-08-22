using System;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using Xunit;

namespace TC1.RepairShop.UnitTests.ServiceOrders;

public class ServiceOrderPartTests
{
    [Fact]
    public void Create_ShouldInitializeServiceOrderPart_WithCustomerSupplied_ResetUnitPriceToZero()
    {
        var part = ServiceOrderPart.Create(Guid.NewGuid(), Guid.NewGuid(), 0, 2, true);

        Assert.NotEqual(Guid.Empty, part.Id);
        Assert.True(part.SuppliedByCustomer);
    }

    [Fact]
    public void Create_ShouldInitializeServiceOrderPart_WithSupplierPrice()
    {
        var part = ServiceOrderPart.Create(Guid.NewGuid(), Guid.NewGuid(),0, 3, false);

        Assert.NotEqual(Guid.Empty, part.Id);
        Assert.False(part.SuppliedByCustomer);
    }
}
