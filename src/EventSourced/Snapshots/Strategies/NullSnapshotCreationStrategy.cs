using EventSourced.Domain;

namespace EventSourced.Snapshots.Strategies
{
    class NullSnapshotCreationStrategy : ISnapshotCreationStrategy
    {
        public bool ShouldCreateSnapshot<TAggregateRoot>(TAggregateRoot root) where TAggregateRoot : AggregateRoot
        {
            return false;
        }
    }
}