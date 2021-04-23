namespace EventSourced.ExternalEvents.API.Configuration
{
    public class EventSourcedExternalWebApiOptions
    {
        public string ExternalEventsEndpoint { get; }

        public EventSourcedExternalWebApiOptions(string externalEventsEndpoint)
        {
            ExternalEventsEndpoint = externalEventsEndpoint;
        }
    }
}