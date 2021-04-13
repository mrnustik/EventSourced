using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Abstractions.Domain.Events;

namespace EventSourced.Persistence.Abstractions
{
    public interface IEventStore
    {
        Task StoreEventsAsync(string streamId, Type aggregateRootType, IList<IDomainEvent> domainEvents, CancellationToken ct);
        Task<IDomainEvent[]> GetByStreamIdAsync(string streamId, Type aggregateRootType, CancellationToken ct);
        Task<IDictionary<string, IDomainEvent[]>> GetAllStreamsOfType(Type aggregateRootType, CancellationToken ct);
    }
}