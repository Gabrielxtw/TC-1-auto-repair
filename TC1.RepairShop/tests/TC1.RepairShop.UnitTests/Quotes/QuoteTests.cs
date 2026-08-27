using System;
using Xunit;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.UnitTests.Quotes;

public class QuoteTests
{
    [Fact]
    public void Create_ShouldInitializeQuote()
    {
        var serviceOrderId = Guid.NewGuid();
        var quote = Quote.Create(serviceOrderId, 1000m);

        Assert.NotEqual(Guid.Empty, quote.Id);
        Assert.Equal(serviceOrderId, quote.ServiceOrderId);
        Assert.Equal(1000m, quote.Price);
        Assert.Equal(QuoteStatus.Draft, quote.QuoteStatusValue);
        Assert.Equal(0, quote.RejectionCount);
        Assert.Equal(Status.Active, quote.Status);
    }

    [Fact]
    public void Reject_ShouldSetRejectedAndIncrementCount()
    {
        var quote = Quote.Create(Guid.NewGuid(), 200m);

        quote.Reject();

        Assert.Equal(QuoteStatus.UnderReview, quote.QuoteStatusValue);
        Assert.Equal(1, quote.RejectionCount);
    }

    [Fact]
    public void Approve_ShouldSetApproved()
    {
        var quote = Quote.Create(Guid.NewGuid(), 200m);

        quote.Approve();

        Assert.Equal(QuoteStatus.Approved, quote.QuoteStatusValue);
    }

    [Fact]
    public void Delete_ShouldSetStatusDeleted()
    {
        var quote = Quote.Create(Guid.NewGuid(), 200m);

        quote.Delete();

        Assert.Equal(Status.Deleted, quote.Status);
    }
}
