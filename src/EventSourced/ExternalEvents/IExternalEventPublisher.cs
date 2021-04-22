using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.ExternalEvents
{
    public interface IExternalEventPublisher
    {
        Task PublishExternalEventAsync(string eventType, string eventData, CancellationToken ct);
    }
}