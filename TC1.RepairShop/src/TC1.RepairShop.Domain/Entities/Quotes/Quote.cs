using TC1.RepairShop.Domain.Entities.Common;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Enums;

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

    public void Reject()
    {
        QuoteStatusValue = QuoteStatus.Rejected;
        RejectionCount++;
    }

    public void Approve()
    {
        QuoteStatusValue = QuoteStatus.Approved;
    }
}
