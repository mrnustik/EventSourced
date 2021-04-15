using System;
using System.ComponentModel.DataAnnotations;

namespace EventSourced.Persistence.EntityFramework.Entities
{
    public class AggregateBasedProjectionEntity
    {
        public Guid AggregateRootId { get; set; }
        public string SerializedProjectionType { get; set; } = null!;
        public string SerializedProjectionData { get; set; } = null!;
    }
}