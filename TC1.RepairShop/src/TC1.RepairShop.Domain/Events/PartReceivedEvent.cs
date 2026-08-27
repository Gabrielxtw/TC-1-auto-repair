using System;

namespace TC1.RepairShop.Domain.Events
{
    public sealed class PartReceivedEvent : IDomainEvent
    {
        public PartReceivedEvent(Guid partId, int quantity)
        {
            PartId = partId;
            Quantity = quantity;
            OccurredOn = DateTime.UtcNow;
        }

        public Guid PartId { get; }
        public int Quantity { get; }
        public DateTime OccurredOn { get; }
    }
}
