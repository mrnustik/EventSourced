using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Configuration;
using EventSourced.Diagnostics.Web.Model.ExternalEvents;
using EventSourced.ExternalEvents;

namespace EventSourced.Diagnostics.Web.Services
{
    class ExternalEventInformationService : IExternalEventInformationService
    {
        private readonly ExternalEventsOptions _externalEventsOptions;
        private readonly IExternalEventPublisher _externalEventPublisher;

        public ExternalEventInformationService(ExternalEventsOptions externalEventsOptions, IExternalEventPublisher externalEventPublisher)
        {
            _externalEventsOptions = externalEventsOptions;
            _externalEventPublisher = externalEventPublisher;
        }

        public ICollection<ExternalEventModel> GetAllRegisteredExternalEvents()
        {
            return _externalEventsOptions.RegisteredExternalEvents.Select(t => new ExternalEventModel(t))
                                         .ToList();
        }

        public async Task TriggerAsync(string externalEventType, string externalEventData, CancellationToken ct)
        {
            await _externalEventPublisher.PublishExternalEventAsync(externalEventType, externalEventData, ct);
        }
    }
}