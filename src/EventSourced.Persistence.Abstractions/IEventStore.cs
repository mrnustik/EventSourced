using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Abstractions.Domain.Events;

namespace EventSourced.Persistence.Abstractions
{
    public interface IEventStore
    {
        Task StoreEventsAsync(IList<IDomainEvent> domainEvents, CancellationToken ct);
        Task<IList<IDomainEvent>> GetByStreamIdAsync(Guid streamId, CancellationToken ct);
    }
}