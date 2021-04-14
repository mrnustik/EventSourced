using System;

namespace EventSourced.Persistence.EntityFramework.Entities
{
    public class AggregateSnapshotEntity
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string AggregateRootType { get; set; } = null!;
        public string SerializedAggregateState { get; set; } = null!;
    }
}