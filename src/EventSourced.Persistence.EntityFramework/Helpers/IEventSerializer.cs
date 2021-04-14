using System;
using EventSourced.Domain.Events;

namespace EventSourced.Persistence.EntityFramework.Helpers
{
    public interface IEventSerializer
    {
        string SerializeEvent(IDomainEvent domainEvent);
        IDomainEvent DeserializeEvent(string serializedEvent, Type eventType);
    }
}