using System;
using EventSourced.Domain.Events;

namespace EventSourced.Persistence.EntityFramework.Helpers
{
    public interface IEventSerializer
    {
        string SerializeEvent(DomainEvent domainEvent);
        DomainEvent DeserializeEvent(string serializedEvent, Type eventType);
    }
}