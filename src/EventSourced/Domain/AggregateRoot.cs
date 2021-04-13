using System.Collections.Generic;
using EventSourced.Domain.Events;
using EventSourced.Helpers;

namespace EventSourced.Domain
{
    public abstract class AggregateRoot<TId>
        where TId : notnull
    {
        private readonly Queue<IDomainEvent> uncommittedDomainEvents;

        protected AggregateRoot(TId id)
        {
            Id = id;
            uncommittedDomainEvents = new Queue<IDomainEvent>();
        }

        public TId Id { get; }

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