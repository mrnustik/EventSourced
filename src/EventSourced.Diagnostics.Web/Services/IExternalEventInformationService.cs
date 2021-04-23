using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Diagnostics.Web.Model.ExternalEvents;

namespace EventSourced.Diagnostics.Web.Services
{
    public interface IExternalEventInformationService
    {
        ICollection<ExternalEventModel> GetAllRegisteredExternalEvents();
        Task TriggerAsync(string externalEventType, string externalEventData, CancellationToken ct);
    }
}