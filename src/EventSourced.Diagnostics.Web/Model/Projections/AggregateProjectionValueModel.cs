using System;
using Newtonsoft.Json;

namespace EventSourced.Diagnostics.Web.Model.Projections
{
    public class AggregateProjectionValueModel
    {
        public Guid AggregateId { get; }
        public string SerializedProjection { get; }

        public AggregateProjectionValueModel(Guid aggregateId, string serializedProjection)
        {
            AggregateId = aggregateId;
            SerializedProjection = serializedProjection;
        }
    }
}