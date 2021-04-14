using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;
using EventSourced.Helpers;
using EventSourced.Persistence;

namespace EventSourced.Projections
{
    internal class ManualProjectionBuilder : IManualProjectionBuilder
    {
        private readonly IEventStore _eventStore;

        public ManualProjectionBuilder(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<TProjection> BuildProjectionAsync<TProjection>(CancellationToken ct) where TProjection : new()
        {
            var projectionType = typeof(TProjection);
            var buildProjection = await BuildProjectionAsync(projectionType, ct);
            return (TProjection) buildProjection;
        }

        public async Task<object> BuildProjectionAsync(Type projectionType, CancellationToken ct)
        {
            var types = ReflectionHelpers.GetTypesOfDomainEventsApplicableToObject(projectionType);
            var projection = Activator.CreateInstance(projectionType)!;
            foreach (var type in types)
            {
                var events = await _eventStore.GetEventsOfTypeAsync(type, ct);
                projection.ApplyEventsToObject(events);
            }
            return projection;
        }

        public async Task<TAggregateProjection> BuildAggregateProjection<TAggregateProjection, TAggregateRoot, TAggregateRootId>(
            TAggregateRootId id,
            CancellationToken ct)
            where TAggregateProjection : new()
            where TAggregateRootId : notnull
            where TAggregateRoot : AggregateRoot<TAggregateRootId>
        {
            var types = ReflectionHelpers.GetTypesOfDomainEventsApplicableToObject(typeof(TAggregateProjection));
            var allEvents = await _eventStore.GetByStreamIdAsync(id.ToString(), typeof(TAggregateRoot), ct);
            var applicableEvents = allEvents.Where(e => types.Contains(e.GetType()));
            var projection = new TAggregateProjection();
            projection.ApplyEventsToObject(applicableEvents.ToArray());
            return projection;
        }
    }
}