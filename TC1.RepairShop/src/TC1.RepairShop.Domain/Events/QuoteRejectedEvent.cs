using System;

namespace TC1.RepairShop.Domain.Events
{
    public sealed class QuoteRejectedEvent : IDomainEvent
    {
        public QuoteRejectedEvent(Guid quoteId, Guid serviceOrderId)
        {
            QuoteId = quoteId;
            ServiceOrderId = serviceOrderId;
            OccurredOn = DateTime.UtcNow;
        }

        public Guid QuoteId { get; }
        public Guid ServiceOrderId { get; }
        public DateTime OccurredOn { get; }
    }
}
