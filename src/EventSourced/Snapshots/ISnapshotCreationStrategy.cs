using EventSourced.Domain;

namespace EventSourced.Snapshots
{
    public interface ISnapshotCreationStrategy
    {
        bool ShouldCreateSnapshot<TAggregateRoot>(TAggregateRoot root)
            where TAggregateRoot : AggregateRoot;
    }
}