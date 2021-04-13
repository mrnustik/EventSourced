using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Domain.Events
{
    public interface IDomainEventHandler
    {
        Task HandleDomainEventAsync(IDomainEvent domainEvent, CancellationToken ct);
    }
}