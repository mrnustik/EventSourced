using System;
using EventSourced.Domain.Events;

namespace EventSourced.Sample.Warehouse.Domain.ImportLocation.Events
{
    public class ImportLocationCreatedDomainEvent : DomainEvent
    {
        public Guid ImportLocationId { get; }

        public ImportLocationCreatedDomainEvent(Guid importLocationId)
        {
            ImportLocationId = importLocationId;
        }
    }
}