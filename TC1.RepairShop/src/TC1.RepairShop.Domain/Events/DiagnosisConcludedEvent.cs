using System;

namespace TC1.RepairShop.Domain.Events
{
    public sealed class DiagnosisConcludedEvent : IDomainEvent
    {
        public DiagnosisConcludedEvent(Guid serviceOrderId, decimal price, Guid? quoteId)
        {
            ServiceOrderId = serviceOrderId;
            Price = price;
            QuoteId = quoteId;
            OccurredOn = DateTime.UtcNow;
        }

        public Guid ServiceOrderId { get; }
        public decimal Price { get; }
        public Guid? QuoteId { get; set; }
        public DateTime OccurredOn { get; }
    }
}
