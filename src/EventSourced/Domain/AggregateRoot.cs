using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EventSourced.Abstractions.Domain.Events;
using EventSourced.Domain.Events;
using EventSourced.Helpers;

namespace EventSourced.Domain
{
    public abstract class AggregateRoot<TId>
        where TId : notnull
    {
        public TId Id { get; }
        
        private readonly Queue<IDomainEvent> uncommittedDomainEvents;

        protected AggregateRoot(TId id)
        {
            Id = id;
            uncommittedDomainEvents = new Queue<IDomainEvent>();
        }

        public IList<IDomainEvent> DequeueDomainEvents()
        {
            return uncommittedDomainEvents.DequeueAll();
        }

        protected void EnqueueAndApplyEvent(IDomainEvent domainEvent)
        {
            uncommittedDomainEvents.Enqueue(domainEvent);
            TryToApplyEvent(domainEvent);
        }

        private void TryToApplyEvent(IDomainEvent domainEvent)
        {
            this.ApplyEventsToObject(domainEvent);
        }
    }
}