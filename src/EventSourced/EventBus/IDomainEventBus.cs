using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain.Events;

namespace EventSourced.EventBus
{
    public interface IDomainEventBus
    {
        Task PublishDomainEventAsync(DomainEvent domainEvent, CancellationToken ct);
        Task PublishDomainEventsAsync(IEnumerable<DomainEvent> domainEvent, CancellationToken ct);
    }
}