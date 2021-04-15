using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Diagnostics.Web.Model;
using EventSourced.Persistence;

namespace EventSourced.Diagnostics.Web.Services
{
    public class AggregateInformationService : IAggregateInformationService
    {
        private readonly IEventStore _eventStore;

        public AggregateInformationService(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<ICollection<AggregateTypesListItemModel>> GetStoredAggregateTypesAsync(CancellationToken ct)
        {
            var types = await _eventStore.GetAllAggregateTypes(ct);
            return types.Select(t => new AggregateTypesListItemModel(t))
                        .ToList();
        }

        public async Task<ICollection<AggregateInstancesListItemModel>> GetStoredAggregatesOfType(Type aggregateType, CancellationToken ct)
        {
            var streams = await _eventStore.GetAllStreamsOfType(aggregateType, ct);
            var aggregateIds = streams.Keys;
            return aggregateIds.Select(i => new AggregateInstancesListItemModel(i))
                               .ToList();
        }
    }
}