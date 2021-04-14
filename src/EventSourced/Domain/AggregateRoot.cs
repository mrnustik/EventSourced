using System;
using System.Collections.Generic;
using System.Linq;
using EventSourced.Domain.Events;
using EventSourced.Helpers;

namespace EventSourced.Domain
{
    public abstract class AggregateRoot
    {
        private readonly Queue<IDomainEvent> uncommittedDomainEvents;
        public int Version { get; internal set; }

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
            domainEvent.Version = GetNextEventVersion(); 
            ApplyEvent(domainEvent);
        }

        private int GetNextEventVersion()
        {
            var lastAppliedEventVersion = uncommittedDomainEvents.LastOrDefault()?.Version ?? Version;
            return lastAppliedEventVersion + 1;
        }

        internal void RebuildFromEvents(IEnumerable<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                ApplyEvent(domainEvent);
                Version = domainEvent.Version;
            }
        }

        private void ApplyEvent(IDomainEvent domainEvent)
        {
            this.ApplyEventsToObject(domainEvent);
        }
    }
}