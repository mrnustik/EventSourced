using System;
using EventSourced.Domain.Events;

namespace EventSourced.Sample.Warehouse.Domain.Container.Events
{
    public class ReceivedItemFromImportLocationDomainEvent : DomainEvent
    {
        public Guid WarehouseItemId { get; }
        public int Amount { get;}

        public ReceivedItemFromImportLocationDomainEvent(Guid warehouseItemId, int amount)
        {
            WarehouseItemId = warehouseItemId;
            Amount = amount;
        }
    }
}