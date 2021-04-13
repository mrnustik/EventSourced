using System.Threading;
using System.Threading.Tasks;
using EventSourced.Helpers;
using EventSourced.Persistence.Abstractions;

namespace EventSourced.Projections
{
    class ManualProjectionBuilder : IManualProjectionBuilder
    {
        private readonly IEventStore _eventStore;

        public ManualProjectionBuilder(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<TProjection> BuildProjectionAsync<TProjection>(CancellationToken ct) where TProjection : new()
        {
            var types = ReflectionHelpers.GetTypesOfDomainEventsApplicableToObject(typeof(TProjection));
            var projection = new TProjection();
            foreach (var type in types)
            {
                var events = await _eventStore.GetEventsOfTypeAsync(type, ct);
                projection.ApplyEventsToObject(events);
            }
            return projection;
        }
    }
}