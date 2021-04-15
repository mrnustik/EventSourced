using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain.Events;

namespace EventSourced.Persistence
{
    public interface IEventStore
    {
        Task StoreEventsAsync(Guid streamId, Type aggregateRootType, IList<IDomainEvent> domainEvents, CancellationToken ct);
        Task<IDomainEvent[]> GetByStreamIdAsync(Guid streamId, Type aggregateRootType, int fromEventVersion, CancellationToken ct);
        Task<bool> StreamExistsAsync(Guid streamId, Type aggregateRootType, CancellationToken ct);
        Task<IDictionary<Guid, IDomainEvent[]>> GetAllStreamsOfType(Type aggregateRootType, CancellationToken ct);
        Task<IDomainEvent[]> GetEventsOfTypeAsync(Type eventType, CancellationToken ct);
    }
}