using System;
using EventSourced.Sample.Warehouse.Domain.ImportLocation.Events;

namespace EventSourced.Sample.Warehouse.Domain.ImportLocation.Projections
{
    public class ImportLocationProjection
    {
        public Guid ImportLocationId { get; private set; }

        private void Apply(ImportLocationCreatedDomainEvent domainEvent)
        {
            ImportLocationId = domainEvent.ImportLocationId;
        }
    }
}