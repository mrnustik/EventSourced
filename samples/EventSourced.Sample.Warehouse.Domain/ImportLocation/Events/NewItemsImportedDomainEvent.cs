using System;
using EventSourced.Domain.Events;

namespace EventSourced.Sample.Warehouse.Domain.ImportLocation.Events
{
    public class NewItemsImportedDomainEvent : DomainEvent
    {
        public Guid WarehouseItemId { get; }
        public int Amount { get; }

        public NewItemsImportedDomainEvent(Guid warehouseItemId, int amount)
        {
            WarehouseItemId = warehouseItemId;
            Amount = amount;
        }
    }
}