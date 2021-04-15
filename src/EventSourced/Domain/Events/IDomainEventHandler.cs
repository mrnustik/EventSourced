using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Domain.Events
{
    public interface IDomainEventHandler<in TDomainEvent>
        where TDomainEvent : DomainEvent
    {
        Task HandleDomainEventAsync(TDomainEvent domainEvent, CancellationToken ct);
    }
}