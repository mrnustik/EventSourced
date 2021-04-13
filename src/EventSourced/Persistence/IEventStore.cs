using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain.Events;

namespace EventSourced.Persistence
{
    public interface IEventStore
    {
        Task StoreEventsAsync(string streamId, Type aggregateRootType, IList<IDomainEvent> domainEvents, CancellationToken ct);
        Task<IDomainEvent[]> GetByStreamIdAsync(string streamId, Type aggregateRootType, CancellationToken ct);
        Task<IDictionary<string, IDomainEvent[]>> GetAllStreamsOfType(Type aggregateRootType, CancellationToken ct);
        Task<IDomainEvent[]> GetEventsOfTypeAsync(Type type, CancellationToken ct);
    }
}