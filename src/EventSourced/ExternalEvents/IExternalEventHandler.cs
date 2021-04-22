using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.ExternalEvents
{
    public interface IExternalEventHandler<in TExternalEvent>
        where TExternalEvent : class
    {
        Task HandleAsync(TExternalEvent externalEvent, CancellationToken ct);
    }
}