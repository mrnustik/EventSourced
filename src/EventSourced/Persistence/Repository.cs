using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;
using EventSourced.Domain.Events;
using EventSourced.Helpers;

namespace EventSourced.Persistence
{
    public class Repository<TAggregateRoot, TAggregateRootId> : IRepository<TAggregateRoot, TAggregateRootId>
        where TAggregateRoot : AggregateRoot<TAggregateRootId>
        where TAggregateRootId : notnull
    {
        private readonly IEventStore _eventStore;
        private readonly IEnumerable<IDomainEventHandler> _domainEventHandlers;

        public Repository(IEventStore eventStore, IEnumerable<IDomainEventHandler> domainEventHandlers)
        {
            _eventStore = eventStore;
            _domainEventHandlers = domainEventHandlers;
        }

        public async Task SaveAsync(TAggregateRoot aggregateRoot, CancellationToken ct)
        {
            var newDomainEvents = aggregateRoot.DequeueDomainEvents();
            await _eventStore.StoreEventsAsync(aggregateRoot.Id.ToString(), typeof(TAggregateRoot), newDomainEvents, ct);
            await InvokeDomainEventHandlersAsync(newDomainEvents, ct);
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
                var aggregateRootId = streamId.ToAggregateRootId<TAggregateRootId>();
                var aggregateRoot = ConstructAggregateRoot(aggregateRootId);
                aggregateRoot.ApplyEventsToObject(events);
                aggregateCollection.Add(aggregateRoot);
            }
            return aggregateCollection;
        }

        private async Task InvokeDomainEventHandlersAsync(IList<IDomainEvent> domainEvents, CancellationToken ct)
        {
            foreach (var domainEventHandler in _domainEventHandlers)
            {
                foreach (var domainEvent in domainEvents)
                {
                    await domainEventHandler.HandleDomainEventAsync(domainEvent, ct);
                }
            }
        }

        private static TAggregateRoot ConstructAggregateRoot(TAggregateRootId id)
        {
            var aggregateRoot = (TAggregateRoot) Activator.CreateInstance(typeof(TAggregateRoot), id)!;
            return aggregateRoot;
        }
    }
}