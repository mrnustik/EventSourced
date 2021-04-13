using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;
using EventSourced.Helpers;

namespace EventSourced.Persistence
{
    public class Repository<TAggregateRoot, TAggregateRootId> : IRepository<TAggregateRoot, TAggregateRootId>
        where TAggregateRoot : AggregateRoot<TAggregateRootId>
        where TAggregateRootId : notnull
    {
        private readonly IEventStore _eventStore;

        public Repository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public Task SaveAsync(TAggregateRoot aggregateRoot, CancellationToken ct)
        {
            var newDomainEvents = aggregateRoot.DequeueDomainEvents();
            return _eventStore.StoreEventsAsync(aggregateRoot.Id.ToString(), typeof(TAggregateRoot), newDomainEvents, ct);
        }

        public async Task<TAggregateRoot> GetByIdAsync(TAggregateRootId id, CancellationToken ct)
        {
            var domainEvents = await _eventStore.GetByStreamIdAsync(id.ToString(), typeof(TAggregateRoot), ct);
            var aggregateRoot = ConstructAggregateRoot(id);
            aggregateRoot.ApplyEventsToObject(domainEvents);
            return aggregateRoot;
        }


        public async Task<ICollection<TAggregateRoot>> GetAllAsync(CancellationToken ct)
        {
            var aggregateToEventsMap = await _eventStore.GetAllStreamsOfType(typeof(TAggregateRoot), ct);
            var aggregateCollection = new List<TAggregateRoot>();
            foreach (var (streamId, events) in aggregateToEventsMap)
            {
                var aggregateRootId = MapStringStreamIdToAggregateRootId(streamId);
                var aggregateRoot = ConstructAggregateRoot(aggregateRootId);
                aggregateRoot.ApplyEventsToObject(events);
                aggregateCollection.Add(aggregateRoot);
            }
            return aggregateCollection;
        }

        private TAggregateRootId MapStringStreamIdToAggregateRootId(string streamId)
        {
            var idType = typeof(TAggregateRootId);
            if (idType.IsPrimitive) return (TAggregateRootId) Convert.ChangeType(streamId, idType);
            return (TAggregateRootId) Activator.CreateInstance(typeof(TAggregateRootId), streamId)!;
        }

        private static TAggregateRoot ConstructAggregateRoot(TAggregateRootId id)
        {
            var aggregateRoot = (TAggregateRoot) Activator.CreateInstance(typeof(TAggregateRoot), id)!;
            return aggregateRoot;
        }
    }
}