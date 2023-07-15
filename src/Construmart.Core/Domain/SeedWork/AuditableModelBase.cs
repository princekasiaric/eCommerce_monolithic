using System;
using Ardalis.GuardClauses;

namespace Construmart.Core.Domain.SeedWork
{
    public abstract class AuditableModelBase : ModelBase
    {
        public DateTime DateCreated { get; private set; }
        public DateTime? DateUpdated { get; private set; }
        public long? CreatedByUserId { get; private set; }
        public long? UpdatedByUserId { get; private set; }

        public virtual void Audit(long? userId, bool isCreate)
        {
            var _ = isCreate == true ?
                CreatedByUserId = userId.HasValue ? Guard.Against.NegativeOrZero(userId.Value, nameof(userId)) : null
                : UpdatedByUserId = userId.HasValue ? Guard.Against.NegativeOrZero(userId.Value, nameof(userId)) : null;
        }
    }
}