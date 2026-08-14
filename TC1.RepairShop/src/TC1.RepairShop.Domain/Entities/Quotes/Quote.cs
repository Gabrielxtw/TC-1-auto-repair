using TC1.RepairShop.Domain.Entities.Common;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Domain.Entities.Quotes;

public class Quote: BaseEntity
{
    public Guid ServiceOrderId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal Discount { get; private set; }
    public decimal FinalPrice { get; private set; }
    public QuoteStatus QuoteStatusValue { get; private set; }
    public int RejectionCount { get; private set; }

    private Quote()
    {
    }

    public static Quote Create(Guid serviceOrderId, decimal totalAmount, decimal discount = 0)
    {
        var quote = new Quote
        {
            ServiceOrderId = serviceOrderId,
            QuoteStatusValue = QuoteStatus.Draft,
            RejectionCount = 0,
        };

        quote.SetAmount(totalAmount, discount);

        return quote;
    }

    public void SetAmount(decimal totalAmount, decimal discount)
    {
        TotalAmount = totalAmount;
        Discount = discount;
        FinalPrice = totalAmount - totalAmount * discount / 100;
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
