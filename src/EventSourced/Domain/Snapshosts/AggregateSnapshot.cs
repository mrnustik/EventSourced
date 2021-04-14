using System;

namespace EventSourced.Domain.Snapshosts
{
    public class AggregateSnapshot<TAggregateRoot> where TAggregateRoot : AggregateRoot
    {
        public Guid Id { get; }
        public int Version { get; }
        public TAggregateRoot AggregateState { get; set; }

        public AggregateSnapshot(Guid id, int version, TAggregateRoot aggregateState)
        {
            Id = id;
            Version = version;
            AggregateState = aggregateState;
        }
    }
}