using System.ComponentModel.DataAnnotations;

namespace EventSourced.Persistence.EntityFramework.Entities
{
    public class TypeBasedProjectionEntity
    {
        [Key] public string SerializedProjectionType { get; set; } = null!;
        public string SerializedProjectionData { get; set; } = null!;
    }
}