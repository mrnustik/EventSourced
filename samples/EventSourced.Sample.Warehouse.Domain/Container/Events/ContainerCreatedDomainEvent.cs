using System;
using EventSourced.Domain.Events;

namespace EventSourced.Sample.Warehouse.Domain.Container.Events
{
    public class ContainerCreatedDomainEvent : DomainEvent
    {
        public Guid Id { get; }
        public string Identifier { get; }

        public ContainerCreatedDomainEvent(Guid id, string identifier)
        {
            Identifier = identifier;
            Id = id;
        }
    }
}