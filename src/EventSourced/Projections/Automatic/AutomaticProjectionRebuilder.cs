using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Configuration;
using EventSourced.Helpers;
using EventSourced.Persistence;

namespace EventSourced.Projections.Automatic
{
    public class AutomaticProjectionRebuilder : IAutomaticProjectionRebuilder
    {
        private readonly AutomaticProjectionOptions _automaticProjectionOptions;
        private readonly IEventStore _eventStore;
        private readonly IManualProjectionBuilder _manualProjectionBuilder;
        private readonly IProjectionStore _projectionStore;

        public AutomaticProjectionRebuilder(AutomaticProjectionOptions automaticProjectionOptions,
                                            IManualProjectionBuilder manualProjectionBuilder,
                                            IProjectionStore projectionStore,
                                            IEventStore eventStore)
        {
            _automaticProjectionOptions = automaticProjectionOptions;
            _manualProjectionBuilder = manualProjectionBuilder;
            _projectionStore = projectionStore;
            _eventStore = eventStore;
        }

        public async Task RebuildAllRegisteredAutomaticProjections(CancellationToken ct)
        {
            await RebuildAllAutomaticProjectionsAsync(ct);
            await RebuildAllAggregateProjectionsAsync(ct);
        }

        private async Task RebuildAllAggregateProjectionsAsync(CancellationToken ct)
        {
            foreach (var aggregateProjectionType in _automaticProjectionOptions.RegisteredAutomaticAggregateProjections)
            {
                var aggregateRootType = ReflectionHelpers.GetAggregateRootTypeFromProjection(aggregateProjectionType);
                var allStreamsMap = await _eventStore.GetAllStreamsOfType(aggregateRootType, ct);
                foreach (var (aggregateRootId, domainEvents) in allStreamsMap)
                {
                    var aggregateProjection = Activator.CreateInstance(aggregateProjectionType, aggregateRootId)!;
                    aggregateProjection.ApplyEventsToObject(domainEvents);
                    await _projectionStore.StoreAggregateProjectionAsync(aggregateRootId, aggregateProjection, ct);
                }
            }
        }

        private async Task RebuildAllAutomaticProjectionsAsync(CancellationToken ct)
        {
            foreach (var projectionType in _automaticProjectionOptions.RegisteredAutomaticProjections)
            {
                var projection = await _manualProjectionBuilder.BuildProjectionAsync(projectionType, ct);
                await _projectionStore.StoreProjectionAsync(projection, ct);
            }
        }
    }
}