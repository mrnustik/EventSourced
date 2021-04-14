using EventSourced.Domain;
using EventSourced.Domain.Snapshosts;
using EventSourced.Persistence.EntityFramework.Entities;

namespace EventSourced.Persistence.EntityFramework.Mappers
{
    public interface IAggregateSnapshotEntityMapper
    {
        AggregateSnapshotEntity MapToEntity<TAggregateRoot>(AggregateSnapshot<TAggregateRoot> aggregateSnapshot)
            where TAggregateRoot : AggregateRoot;

        TAggregateRoot MapToAggregateRoot<TAggregateRoot>(AggregateSnapshotEntity entity)
            where TAggregateRoot : AggregateRoot;
    }
}