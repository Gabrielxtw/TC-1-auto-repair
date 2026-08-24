using System;

namespace TC1.RepairShop.Domain.Events
{
    public sealed class QuoteApprovedEvent : IDomainEvent
    {
        public QuoteApprovedEvent(Guid serviceOrderId)
        {
            ServiceOrderId = serviceOrderId;
            OccurredOn = DateTime.UtcNow;
        }

        public Guid ServiceOrderId { get; }
        public DateTime OccurredOn { get; }
    }
}
