using System;
using System.Collections.Generic;
using System.Text;

namespace TC1.RepairShop.Domain.Entities.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }
        public DateTime RegisteredAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime DeletedAt { get; private set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            RegisteredAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
