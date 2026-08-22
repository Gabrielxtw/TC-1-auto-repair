using MediatR;
using System;

namespace TC1.RepairShop.Domain.Events
{
    public interface IDomainEvent: INotification
    {
        DateTime OccurredOn { get; }
    }
}
