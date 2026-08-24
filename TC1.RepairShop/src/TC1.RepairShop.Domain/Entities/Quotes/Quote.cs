using TC1.RepairShop.Domain.Entities.Common;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Events;

namespace TC1.RepairShop.Domain.Entities.Quotes;

public class Quote: BaseEntity
{
    public Guid ServiceOrderId { get; private set; }
    public ServiceOrder ServiceOrder { get; private set; } = null!;
    public decimal Price { get; private set; }
    public QuoteStatus QuoteStatusValue { get; private set; }
    public int RejectionCount { get; private set; }

    private Quote()
    {
    }

    public static Quote Create(Guid serviceOrderId, decimal price)
    {
        var quote = new Quote
        {
            ServiceOrderId = serviceOrderId,
            QuoteStatusValue = QuoteStatus.Draft,
            RejectionCount = 0,
            Price = price
        };

        return quote;
    }
    public void UpdatePrice(decimal newPrice)
    {
        Price = newPrice;

        RaiseDomainEvent(new QuoteCreatedUpdatedEvent(Id));
    }

    public void Reject()
    {
        QuoteStatusValue = QuoteStatus.Rejected;
        RejectionCount++;
        if(RejectionCount >= 3)
        {
            QuoteStatusValue = QuoteStatus.UnderReview;
        }
        RaiseDomainEvent(new QuoteRejectedEvent(
            Id,
            ServiceOrderId)
        );
    }
    public void MarkUnderReview()
    {
        QuoteStatusValue = QuoteStatus.UnderReview;
    }
    public void SendToCustomer()
    {
        QuoteStatusValue = QuoteStatus.SentToCustomer;
    }

    public void Approve()
    {
        QuoteStatusValue = QuoteStatus.Approved;
        RaiseDomainEvent(new QuoteApprovedEvent(
            ServiceOrderId)
        );
    }
}
