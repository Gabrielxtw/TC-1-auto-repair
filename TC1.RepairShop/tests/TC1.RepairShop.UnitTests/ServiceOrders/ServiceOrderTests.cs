using System;
using TC1.RepairShop.Domain.ServiceOrders;
using TC1.RepairShop.Domain.Common;
using Xunit;

namespace TC1.RepairShop.UnitTests.ServiceOrders;

public class ServiceOrderTests
{
    [Fact]
    public void Create_ShouldInitializeServiceOrder()
    {
        var customerId = Guid.NewGuid();
        var vehicleId = Guid.NewGuid();
        var order = ServiceOrder.Create(customerId, vehicleId);

        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal(customerId, order.CustomerId);
        Assert.Equal(vehicleId, order.VehicleId);
        Assert.Equal(ServiceOrderStatus.Received, order.OrderStatusValue);
        Assert.Equal(Status.Active, order.Status);
        Assert.True(order.OpenedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void AttachQuote_ShouldSetQuoteId()
    {
        var order = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());
        var quoteId = Guid.NewGuid();

        order.AttachQuote(quoteId);

        Assert.Equal(quoteId, order.QuoteId);
    }

    [Fact]
    public void AdvanceTo_ShouldUpdateStatusAndSetCompletedAt_WhenDelivered()
    {
        var order = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());

        order.AdvanceTo(ServiceOrderStatus.InProgress);
        Assert.Equal(ServiceOrderStatus.InProgress, order.OrderStatusValue);

        order.AdvanceTo(ServiceOrderStatus.Delivered);
        Assert.Equal(ServiceOrderStatus.Delivered, order.OrderStatusValue);
        Assert.NotNull(order.CompletedAt);
        Assert.True(order.CompletedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Delete_ShouldSetStatusDeleted()
    {
        var order = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());

        order.Delete();

        Assert.Equal(Status.Deleted, order.Status);
    }
}
