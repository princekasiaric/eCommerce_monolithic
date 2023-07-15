using System;
using System.Collections.Generic;
using MediatR;

namespace Construmart.Core.Domain.SeedWork
{
    public abstract class ModelBase
    {
        private List<INotification> _domainEvents;

        public long Id { get; set; }
        public string RowVersion { get; set; }
        public ICollection<INotification> DomainEvents => _domainEvents;

        public void AddDomainEvent(INotification @event)
        {
            _domainEvents ??= new List<INotification>();
            DomainEvents.Add(@event);
        }

        public void RemoveDomainEvent(INotification @event)
        {
            if (_domainEvents is null) return;
            _domainEvents.Remove(@event);
        }
    }
}