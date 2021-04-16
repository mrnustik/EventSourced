using System;

namespace EventSourced.Diagnostics.Web.Model
{
    public class AggregateInstancesListItemModel
    {
        public Guid Id { get; }
        public int Version { get; }
        public string SerializedAggregate { get; }

        public AggregateInstancesListItemModel(Guid id, int version, string serializedAggregate)
        {
            Id = id;
            Version = version;
            SerializedAggregate = serializedAggregate;
        }
    }
}