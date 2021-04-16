using System;

namespace EventSourced.Diagnostics.Web.Model.Aggregates
{
    public class AggregateInstancesListItemModel
    {
        public Guid Id { get; }
        public int Version { get; }
        public string SerializedAggregate { get; }
        public string SerializedEvents { get; }

        public AggregateInstancesListItemModel(Guid id, int version, string serializedAggregate, string serializedEvents)
        {
            Id = id;
            Version = version;
            SerializedAggregate = serializedAggregate;
            SerializedEvents = serializedEvents;
        }
    }
}