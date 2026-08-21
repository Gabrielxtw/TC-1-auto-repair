using System;

namespace TC1.RepairShop.Domain.Events
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
