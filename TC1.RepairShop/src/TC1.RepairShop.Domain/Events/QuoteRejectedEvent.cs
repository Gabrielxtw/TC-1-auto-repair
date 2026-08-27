using System;

namespace TC1.RepairShop.Domain.Events
{
    public sealed class QuoteRejectedEvent : IDomainEvent
    {
        public QuoteRejectedEvent(Guid serviceOrderId)
        {
            ServiceOrderId = serviceOrderId;
            OccurredOn = DateTime.UtcNow;
        }
        public Guid ServiceOrderId { get; }
        public DateTime OccurredOn { get; }
    }
}
