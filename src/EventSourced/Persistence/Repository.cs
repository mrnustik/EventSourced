using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;
using EventSourced.Helpers;
using EventSourced.Persistence.Abstractions;

namespace EventSourced.Persistence
{
    public class Repository<TAggregateRoot, TAggregateRootId> : IRepository<TAggregateRoot, TAggregateRootId>
        where TAggregateRoot : AggregateRoot<TAggregateRootId>, new()
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
            var aggregateRoot = new TAggregateRoot();
            aggregateRoot.ApplyEventsToObject(domainEvents);
            return aggregateRoot;
        }
    }
}