using System;

namespace EventSourced.Domain.Events
{
    public class DomainEvent : IDomainEvent
    {
        public Guid Id { get; }

        public DomainEvent()
        {
            Id = Guid.NewGuid();
        }
    }
}