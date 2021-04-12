using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Abstractions.Domain.Events;

namespace EventSourced.Persistence.Abstractions
{
    public interface IEventStore
    {
        Task StoreEventsAsync(string streamId, IList<IDomainEvent> domainEvents, CancellationToken ct);
        Task<IDomainEvent[]> GetByStreamIdAsync(string streamId, CancellationToken ct);
    }
}