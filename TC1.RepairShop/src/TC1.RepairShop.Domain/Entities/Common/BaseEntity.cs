using MediatR;
using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Events;

namespace TC1.RepairShop.Domain.Entities.Common
{
    public abstract class BaseEntity: INotification
    {
        public Guid Id { get; private set; }
        public Status Status { get; private set; }
        public DateTime RegisteredAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            RegisteredAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Status = Status.Active;
        }

        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        public void ClearDomainEvents() => _domainEvents.Clear();
        public IReadOnlyCollection<IDomainEvent> DequeueDomainEvents()
        {
            var events = _domainEvents.ToList();
            _domainEvents.Clear();
            return events;
        }
        protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
            _domainEvents.Add(domainEvent);

        protected bool IsActive() => Status == Status.Active;

        public virtual void Delete()
        {
            DeletedAt = DateTime.UtcNow;
            Status = Status.Deleted;
        }
        public void Deactivate()
        {
            if (!IsActive())
                throw new BusinessException(BusinessErrors.EntityErrors.CannotDoActionInactiveEntity);

            Status = Status.Inactive;
        }
        public void Activate()
        {
            Status = Status.Active;
        }
    }
}
