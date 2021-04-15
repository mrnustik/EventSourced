using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain.Events;

namespace EventSourced.Persistence
{
    public interface IEventStreamUpdatedEventHandler
    {
        Task HandleDomainEventAsync(Type aggregateRootType, Guid aggregateRootId, IDomainEvent domainEvent, CancellationToken ct);
    }
}