using System;

namespace EventSourced.Domain.Events
{
    public abstract class DomainEvent : IDomainEvent
    {
        protected DomainEvent()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public int Version { get; set; }
    }
}