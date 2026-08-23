using System;

namespace TC1.RepairShop.Domain.Events
{
    public sealed class QuoteCreatedUpdatedEvent : IDomainEvent
    {
        public QuoteCreatedUpdatedEvent(Guid quoteId)
        {
            QuoteId = quoteId;
            OccurredOn = DateTime.UtcNow;
        }

        public Guid QuoteId { get; }
        public DateTime OccurredOn { get; }
    }
}
