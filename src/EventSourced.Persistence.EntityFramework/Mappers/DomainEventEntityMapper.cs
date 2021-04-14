using System;
using EventSourced.Domain.Events;
using EventSourced.Persistence.EntityFramework.Entities;
using EventSourced.Persistence.EntityFramework.Helpers;

namespace EventSourced.Persistence.EntityFramework.Mappers
{
    public class DomainEventEntityMapper : IDomainEventEntityMapper
    {
        private readonly ITypeSerializer _typeSerializer;
        private readonly IEventSerializer _eventSerializer;

        public DomainEventEntityMapper(ITypeSerializer typeSerializer, IEventSerializer eventSerializer)
        {
            _typeSerializer = typeSerializer;
            _eventSerializer = eventSerializer;
        }

        public DomainEventEntity MapToEntity(IDomainEvent domainEvent, Guid streamId, Type aggregateRootType)
        {
            return new()
            {
                StreamId = streamId,
                AggregateRootType = _typeSerializer.SerializeType(aggregateRootType),
                EventType = _typeSerializer.SerializeType(domainEvent.GetType()),
                SerializedEvent = _eventSerializer.SerializeEvent(domainEvent)
            };
        }

        public IDomainEvent MapToDomainEvent(DomainEventEntity domainEventEntity)
        {
            var eventType = _typeSerializer.DeserializeType(domainEventEntity.EventType);
            return _eventSerializer.DeserializeEvent(domainEventEntity.SerializedEvent, eventType);
        }
    }
}