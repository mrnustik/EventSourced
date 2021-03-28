using System;
using EventSourced.Abstractions.Domain.Events;

namespace EventSourced.Domain.Events
{
    public class DomainEvent : IDomainEvent
    {
        public Guid Id { get; }

        protected DomainEvent()
        {
            Id = Guid.NewGuid();
        }
    }
}