using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;
using EventSourced.Domain.Snapshosts;
using EventSourced.Persistence.InMemory.Helpers;

namespace EventSourced.Persistence.InMemory
{
    public class InMemorySnapshotStore<TAggregateRoot> : ISnapshotStore<TAggregateRoot> 
        where TAggregateRoot : AggregateRoot
    {
        private ConcurrentDictionary<Guid, AggregateSnapshot<TAggregateRoot>> Snapshots { get; }

        public InMemorySnapshotStore()
            : this(new Dictionary<Guid, AggregateSnapshot<TAggregateRoot>>())
        {
        }

        public InMemorySnapshotStore(IDictionary<Guid, AggregateSnapshot<TAggregateRoot>> snapshots)
        {
            Snapshots = new ConcurrentDictionary<Guid, AggregateSnapshot<TAggregateRoot>>(snapshots);
        }

        public Task StoreSnapshotAsync(TAggregateRoot aggregateRoot, CancellationToken ct)
        {
            var aggregateSnapshot = new AggregateSnapshot<TAggregateRoot>(aggregateRoot.Id, aggregateRoot.Version, aggregateRoot.DeepCloneGeneric());
            Snapshots[aggregateRoot.Id] = aggregateSnapshot;
            return Task.CompletedTask;
        }

        public Task<TAggregateRoot?> LoadSnapshotAsync(Guid aggregateRootId, CancellationToken ct)
        {
            if (Snapshots.TryGetValue(aggregateRootId, out var aggregateSnapshot))
            {
                return Task.FromResult<TAggregateRoot?>(aggregateSnapshot.AggregateState);
            }
            else
            {
                return Task.FromResult<TAggregateRoot?>(null);
            }
        }
    }
}