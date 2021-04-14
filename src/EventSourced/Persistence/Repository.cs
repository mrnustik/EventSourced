using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;
using EventSourced.Domain.Events;
using EventSourced.Helpers;

namespace EventSourced.Persistence
{
    public class Repository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : AggregateRoot
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
            await _eventStore.StoreEventsAsync(aggregateRoot.Id, typeof(TAggregateRoot), newDomainEvents, ct);
            await InvokeDomainEventHandlersAsync(aggregateRoot.Id, newDomainEvents, ct);
        }

        public async Task<TAggregateRoot> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var domainEvents = await _eventStore.GetByStreamIdAsync(id, typeof(TAggregateRoot), ct);
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
                var aggregateRootId = streamId;
                var aggregateRoot = ConstructAggregateRoot(aggregateRootId);
                aggregateRoot.ApplyEventsToObject(events);
                aggregateCollection.Add(aggregateRoot);
            }
            return aggregateCollection;
        }

        private async Task InvokeDomainEventHandlersAsync(Guid aggregateRootId, IList<IDomainEvent> domainEvents, CancellationToken ct)
        {
            foreach (var domainEventHandler in _domainEventHandlers)
            {
                foreach (var domainEvent in domainEvents)
                {
                    await domainEventHandler.HandleDomainEventAsync(typeof(TAggregateRoot), aggregateRootId, domainEvent, ct);
                }
            }
        }

        private static TAggregateRoot ConstructAggregateRoot(Guid id)
        {
            var aggregateRoot = (TAggregateRoot) Activator.CreateInstance(typeof(TAggregateRoot), id)!;
            return aggregateRoot;
        }
    }
}