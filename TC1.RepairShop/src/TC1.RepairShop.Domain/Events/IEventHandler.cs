using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace TC1.RepairShop.Domain.Events
{
    public interface IEventHandler<TEvent> : INotificationHandler<TEvent>
        where TEvent : IDomainEvent
    {
        new Task Handle(TEvent domainEvent, CancellationToken cancellationToken = default);
    }
}
