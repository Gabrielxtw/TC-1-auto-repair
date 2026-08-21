using System;

namespace TC1.RepairShop.Domain.Events
{
    public sealed class DiagnosisConcludedEvent : IDomainEvent
    {
        public DiagnosisConcludedEvent(Guid serviceOrderId, ICollection<string> serviceIds, ICollection<string> partIds)
        {
            ServiceOrderId = serviceOrderId;
            ServiceIds = serviceIds;
            PartIds = partIds;
            OccurredOn = DateTime.UtcNow;
        }

        public Guid ServiceOrderId { get; }
        public ICollection<string> ServiceIds { get; } = new HashSet<string>();
        public ICollection<string> PartIds { get; } = new HashSet<string>();
        public DateTime OccurredOn { get; }
    }
}
