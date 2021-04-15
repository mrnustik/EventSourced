using System;
using EventSourced.Domain.Events;

namespace EventSourced.Sample.Warehouse.Domain.WarehouseItem.Events
{
    public class WarehouseItemTitleUpdatedDomainEvent : DomainEvent
    {
        public Guid WarehouseItemId { get; }
        public string NewTitle { get; }

        public WarehouseItemTitleUpdatedDomainEvent(Guid warehouseItemId, string newTitle)
        {
            NewTitle = newTitle;
            WarehouseItemId = warehouseItemId;
        }
    }
}