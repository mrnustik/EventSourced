using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Configuration
{
    public class EventSourcedOptions
    {
        public IServiceCollection ServiceCollection { get; }
        public AutomaticProjectionOptions AutomaticProjectionOptions { get; }
        public ExternalEventsOptions ExternalEventsOptions { get; }

        public EventSourcedOptions(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
            AutomaticProjectionOptions = new AutomaticProjectionOptions();
            ExternalEventsOptions = new ExternalEventsOptions();
        }
    }
}