using System;
using EventSourced.Domain.Events;

namespace EventSourced.Sample.Warehouse.Domain.ImportLocation.Events
{
    public class ItemMovedFromImportLocationDomainEvent : DomainEvent
    {
        public Guid WarehouseItemId { get; }
        public Guid DestinationContainerId { get; }
        public int Amount { get; }

        public ItemMovedFromImportLocationDomainEvent(Guid warehouseItemId, Guid destinationContainerId, int amount)
        {
            WarehouseItemId = warehouseItemId;
            Amount = amount;
            DestinationContainerId = destinationContainerId;
        }
    }
}