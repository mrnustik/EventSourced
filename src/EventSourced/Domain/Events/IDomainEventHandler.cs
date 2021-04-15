using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Domain.Events
{
    public interface IDomainEventHandler<in TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        Task HandleDomainEventAsync(TDomainEvent domainEvent, CancellationToken ct);
    }
}