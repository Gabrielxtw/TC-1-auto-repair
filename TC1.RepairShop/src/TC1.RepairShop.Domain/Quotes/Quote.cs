namespace TC1.RepairShop.Domain.Quotes;

public class Quote
{
    public Guid Id { get; set; }
    public Guid ServiceOrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public QuoteStatus Status { get; set; }
    public int RejectionCount { get; set; }
}
