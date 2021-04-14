using EventSourced.Domain;

namespace EventSourced.Snapshots.Strategies
{
    class EventCountBasedSnapshotCreationStrategy : ISnapshotCreationStrategy
    {
        public int EventCountBetweenSnapshots { get; }

        public EventCountBasedSnapshotCreationStrategy(int eventCountBetweenSnapshots)
        {
            EventCountBetweenSnapshots = eventCountBetweenSnapshots;
        }
        
        public bool ShouldCreateSnapshot<TAggregateRoot>(TAggregateRoot root)
            where TAggregateRoot : AggregateRoot
        {
            return root.Version % EventCountBetweenSnapshots == 0;
        }
    }
}