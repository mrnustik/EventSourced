using EventSourced.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventSourced.Persistence.InMemory.Configuration
{
    public static class EventSourcedOptionsExtensions
    {
        public static EventSourcedOptions UseInMemoryEventStore(this EventSourcedOptions options)
        {
            options.ServiceCollection.AddSingleton<IEventStore, InMemoryEventStore>();
            return options;
        }

        public static EventSourcedOptions UseInMemoryProjectionStore(this EventSourcedOptions options)
        {
            options.ServiceCollection.AddSingleton<IProjectionStore, InMemoryProjectionStore>();
            return options;
        }

        public static EventSourcedOptions UseInMemorySnapshotStore(this EventSourcedOptions options)
        {
            options.ServiceCollection.Replace(new ServiceDescriptor(typeof(ISnapshotStore<>), typeof(InMemorySnapshotStore<>), ServiceLifetime.Scoped));
            return options;
        }
    }
}