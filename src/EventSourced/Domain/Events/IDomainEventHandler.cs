using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Domain.Events
{
    public interface IDomainEventHandler
    {
        Task HandleDomainEventAsync(Type aggregateRootType, Guid aggregateRootId, IDomainEvent domainEvent, CancellationToken ct);
    }
}