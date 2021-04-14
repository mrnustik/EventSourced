using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;
using EventSourced.Domain.Events;
using EventSourced.Exceptions;
using EventSourced.Snapshots;

namespace EventSourced.Persistence
{
    public class Repository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : AggregateRoot
    {
        private readonly IEventStore _eventStore;
        private readonly IEnumerable<IDomainEventHandler> _domainEventHandlers;
        private readonly ISnapshotStore<TAggregateRoot> _snapshotStore;
        private readonly ISnapshotCreationStrategy _snapshotCreationStrategy;

        public Repository(IEventStore eventStore,
                          IEnumerable<IDomainEventHandler> domainEventHandlers,
                          ISnapshotStore<TAggregateRoot> snapshotStore,
                          ISnapshotCreationStrategy snapshotCreationStrategy)
        {
            _eventStore = eventStore;
            _domainEventHandlers = domainEventHandlers;
            _snapshotStore = snapshotStore;
            _snapshotCreationStrategy = snapshotCreationStrategy;
        }

        public async Task SaveAsync(TAggregateRoot aggregateRoot, CancellationToken ct)
        {
            await VerifyAggregateDidNotChangeAsync(aggregateRoot, ct);
            var newDomainEvents = aggregateRoot.DequeueDomainEvents();
            await _eventStore.StoreEventsAsync(aggregateRoot.Id, typeof(TAggregateRoot), newDomainEvents, ct);
            await InvokeDomainEventHandlersAsync(aggregateRoot.Id, newDomainEvents, ct);
            await CreateSnapshotIfNeededAsync(aggregateRoot, ct);
        }

        public async Task<TAggregateRoot> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var aggregateRoot = await LoadFromSnapshotOrCreateAsync(id, ct);
            var domainEvents = await _eventStore.GetByStreamIdAsync(id, typeof(TAggregateRoot), aggregateRoot.Version, ct);
            aggregateRoot.RebuildFromEvents(domainEvents);
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
                aggregateRoot.RebuildFromEvents(events);
                aggregateCollection.Add(aggregateRoot);
            }
            return aggregateCollection;
        }

        private async Task VerifyAggregateDidNotChangeAsync(TAggregateRoot aggregateRoot, CancellationToken ct)
        {
            if (await _eventStore.StreamExistsAsync(aggregateRoot.Id, typeof(TAggregateRoot), ct))
            {
                var existingAggregateRoot = await GetByIdAsync(aggregateRoot.Id, ct);
                if (existingAggregateRoot.Version != aggregateRoot.Version)
                {
                    throw new AggregateVersionConflictException(
                        $"Conflict occured when trying to save aggregate of type {typeof(TAggregateRoot).Name} version {existingAggregateRoot.Version} already exists.");
                }
            }
        }
        
        private async Task InvokeDomainEventHandlersAsync(Guid aggregateRootId, IList<IDomainEvent> domainEvents, CancellationToken ct)
        {
            foreach (var domainEventHandler in _domainEventHandlers)
            foreach (var domainEvent in domainEvents)
            {
                await domainEventHandler.HandleDomainEventAsync(typeof(TAggregateRoot), aggregateRootId, domainEvent, ct);
            }
        }

        private async Task<TAggregateRoot> LoadFromSnapshotOrCreateAsync(Guid id, CancellationToken ct)
        {
            var aggregateFromSnapshot = await _snapshotStore.LoadSnapshotAsync(id, ct);
            return aggregateFromSnapshot ?? ConstructAggregateRoot(id);
        }
        
        private static TAggregateRoot ConstructAggregateRoot(Guid id)
        {
            var aggregateRoot = (TAggregateRoot) Activator.CreateInstance(typeof(TAggregateRoot), id)!;
            return aggregateRoot;
        }

        private async Task CreateSnapshotIfNeededAsync(TAggregateRoot aggregateRoot, CancellationToken ct)
        {
            if (_snapshotCreationStrategy.ShouldCreateSnapshot(aggregateRoot))
            {
                await _snapshotStore.StoreSnapshotAsync(aggregateRoot, ct);
            }
        }
    }
}