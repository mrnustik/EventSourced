using System;
using System.Collections.Generic;
using EventSourced.Domain.Events;
using EventSourced.Helpers;

namespace EventSourced.Domain
{
    public abstract class AggregateRoot
    {
        private readonly Queue<IDomainEvent> uncommittedDomainEvents;

        protected AggregateRoot(Guid id)
        {
            Id = id;
            uncommittedDomainEvents = new Queue<IDomainEvent>();
        }

        public Guid Id { get; }

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