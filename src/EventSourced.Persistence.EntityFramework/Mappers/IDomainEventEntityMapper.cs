using System;
using EventSourced.Domain.Events;
using EventSourced.Persistence.EntityFramework.Entities;

namespace EventSourced.Persistence.EntityFramework.Mappers
{
    public interface IDomainEventEntityMapper
    {
        DomainEventEntity MapToEntity(IDomainEvent domainEvent, Guid streamId, Type aggregateRootType);
        IDomainEvent MapToDomainEvent(DomainEventEntity domainEventEntity);
    }
}