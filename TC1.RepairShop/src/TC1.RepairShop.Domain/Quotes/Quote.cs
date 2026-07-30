using TC1.RepairShop.Domain.Common;

namespace TC1.RepairShop.Domain.Quotes;

public class Quote
{
    public Guid Id { get; private set; }
    public Guid ServiceOrderId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal Discount { get; private set; }
    public decimal FinalPrice { get; private set; }
    public QuoteStatus QuoteStatusValue { get; private set; }
    public int RejectionCount { get; private set; }
    public Status Status { get; private set; }

    private Quote()
    {
    }

    public static Quote Create(Guid serviceOrderId, decimal totalAmount, decimal discount = 0)
    {
        var quote = new Quote
        {
            Id = Guid.NewGuid(),
            ServiceOrderId = serviceOrderId,
            QuoteStatusValue = QuoteStatus.Draft,
            RejectionCount = 0,
            Status = Status.Active,
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

    public void Delete()
    {
        Status = Status.Deleted;
    }
}
