using System;
using System.Collections.Generic;
using System.Text;
using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Domain.Entities.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }
        public Status Status { get; private set; }
        public DateTime RegisteredAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime DeletedAt { get; private set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            RegisteredAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Status = Status.Active;
        }
        private bool IsActive() => Status == Status.Active;

        public virtual void Delete()
        {
            DeletedAt = DateTime.UtcNow;
            Status = Status.Deleted;
        }
        public void Deactivate()
        {
            if (!IsActive())
                throw new BusinessException(BusinessErrors.Entity.CannotDeactivateInactiveEntity);

            Status = Status.Inactive;
        }
        public void Activate()
        {
            Status = Status.Active;
        }
    }
}
