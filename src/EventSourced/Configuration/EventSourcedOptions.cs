using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Configuration
{
    public class EventSourcedOptions
    {
        public EventSourcedOptions(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }

        public IServiceCollection ServiceCollection { get; }
    }
}