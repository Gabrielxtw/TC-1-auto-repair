using System;
using Xunit;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.UnitTests.Quotes;

public class QuoteTests
{
    [Fact]
    public void Create_ShouldInitializeQuoteAndSetAmounts()
    {
        var serviceOrderId = Guid.NewGuid();
        var quote = Quote.Create(serviceOrderId, 1000m, 10m);

        Assert.NotEqual(Guid.Empty, quote.Id);
        Assert.Equal(serviceOrderId, quote.ServiceOrderId);
        Assert.Equal(1000m, quote.TotalAmount);
        Assert.Equal(10m, quote.Discount);
        Assert.Equal(900m, quote.FinalPrice);
        Assert.Equal(QuoteStatus.Draft, quote.QuoteStatusValue);
        Assert.Equal(0, quote.RejectionCount);
        Assert.Equal(Status.Active, quote.Status);
    }

    [Fact]
    public void SetAmount_ShouldUpdateAmounts()
    {
        var quote = Quote.Create(Guid.NewGuid(), 500m, 0);

        quote.SetAmount(800m, 5m);

        Assert.Equal(800m, quote.TotalAmount);
        Assert.Equal(5m, quote.Discount);
        Assert.Equal(760m, quote.FinalPrice);
    }

    [Fact]
    public void Reject_ShouldSetRejectedAndIncrementCount()
    {
        var quote = Quote.Create(Guid.NewGuid(), 200m, 0);

        quote.Reject();

        Assert.Equal(QuoteStatus.Rejected, quote.QuoteStatusValue);
        Assert.Equal(1, quote.RejectionCount);
    }

    [Fact]
    public void Approve_ShouldSetApproved()
    {
        var quote = Quote.Create(Guid.NewGuid(), 200m, 0);

        quote.Approve();

        Assert.Equal(QuoteStatus.Approved, quote.QuoteStatusValue);
    }

    [Fact]
    public void Delete_ShouldSetStatusDeleted()
    {
        var quote = Quote.Create(Guid.NewGuid(), 200m, 0);

        quote.Delete();

        Assert.Equal(Status.Deleted, quote.Status);
    }
}
