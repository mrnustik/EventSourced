namespace EventSourced.ExternalEvents.API.Requests
{
    public record PublishExternalEventRequest(string EventType, string EventData);
}