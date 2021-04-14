using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain.Events;
using EventSourced.Helpers;
using EventSourced.Persistence;

namespace EventSourced.Projections.Automatic
{
    public class AutomaticAggregateProjectionDomainEventHandler : IDomainEventHandler
    {
        private readonly IAutomaticProjectionsEventMapper _automaticProjectionsEventMapper;

        private readonly IProjectionStore _projectionStore;

        public AutomaticAggregateProjectionDomainEventHandler(IProjectionStore projectionStore,
                                                              IAutomaticProjectionsEventMapper automaticProjectionsEventMapper)
        {
            _projectionStore = projectionStore;
            _automaticProjectionsEventMapper = automaticProjectionsEventMapper;
        }

        public async Task HandleDomainEventAsync(Type aggregateRootType,
                                                 Guid aggregateRootId,
                                                 IDomainEvent domainEvent,
                                                 CancellationToken ct)
        {
            var projectionsAffectedByAggregateChange =
                _automaticProjectionsEventMapper.GetProjectionsAffectedByAggregateChange(aggregateRootType);
            foreach (var projectionType in projectionsAffectedByAggregateChange)
            {
                var applicableEventTypes = ReflectionHelpers.GetTypesOfDomainEventsApplicableToObject(projectionType);
                if (applicableEventTypes.Contains(domainEvent.GetType()))
                {
                    var projection = await LoadOrCreateAggregateProjectionOfTypeAsync(projectionType, aggregateRootId, ct);
                    projection.ApplyEventsToObject(domainEvent);
                    await _projectionStore.StoreAggregateProjectionAsync(aggregateRootId, projection, ct);
                }
            }
        }

        private async Task<object> LoadOrCreateAggregateProjectionOfTypeAsync(Type aggregateProjectionType,
                                                                              Guid aggregateRootId,
                                                                              CancellationToken ct)
        {
            var projection = await _projectionStore.LoadAggregateProjectionAsync(aggregateProjectionType, aggregateRootId, ct);
            return projection ?? Activator.CreateInstance(aggregateProjectionType, aggregateRootId)!;
        }
    }
}