using Newtonsoft.Json.Linq;

namespace EventSourced.ExternalEvents.API.Requests
{
    public record PublishExternalEventRequest(string EventType, JObject EventData);
}