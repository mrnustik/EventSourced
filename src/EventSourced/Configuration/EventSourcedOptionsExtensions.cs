using EventSourced.Domain;
using EventSourced.Projections;
using EventSourced.Snapshots;
using EventSourced.Snapshots.Strategies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventSourced.Configuration
{
    public static class EventSourcedOptionsExtensions
    {
        public static EventSourcedOptions RegisterAutomaticProjection<TProjection>(this EventSourcedOptions options)
            where TProjection : new()
        {
            options.AutomaticProjectionOptions.RebuildAutomaticProjectionsOnStart = true;
            options.AutomaticProjectionOptions.RegisteredAutomaticProjections.Add(typeof(TProjection));
            return options;
        }

        public static EventSourcedOptions RegisterAutomaticAggregateProjection<TProjection, TAggregateRoot>(
            this EventSourcedOptions options) where TProjection : AggregateProjection<TAggregateRoot>
            where TAggregateRoot : AggregateRoot
        {
            options.AutomaticProjectionOptions.RebuildAutomaticProjectionsOnStart = true;
            options.AutomaticProjectionOptions.RegisteredAutomaticAggregateProjections.Add(typeof(TProjection));
            return options;
        }

        public static EventSourcedOptions WithEventCountBasedSnapshotStrategy(this EventSourcedOptions options,
                                                                              int numberOfEventBetweenSnapshots)
        {
            options.ServiceCollection.Replace(new ServiceDescriptor(typeof(ISnapshotCreationStrategy),
                                                                    new EventCountBasedSnapshotCreationStrategy(
                                                                        numberOfEventBetweenSnapshots)));
            return options;
        }
    }
}