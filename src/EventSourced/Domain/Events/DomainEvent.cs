using System;

namespace EventSourced.Domain.Events
{
    public class DomainEvent : IDomainEvent
    {
        protected DomainEvent()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
    }
}