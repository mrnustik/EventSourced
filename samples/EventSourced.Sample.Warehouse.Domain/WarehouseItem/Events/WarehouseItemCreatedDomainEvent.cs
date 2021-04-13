using System;
using EventSourced.Domain.Events;

namespace EventSourced.Sample.Warehouse.Domain.WarehouseItem.Events
{
    public class WarehouseItemCreatedDomainEvent : DomainEvent
    {
        public WarehouseItemCreatedDomainEvent(Guid warehouseItemId, string title)
        {
            WarehouseItemId = warehouseItemId;
            Title = title;
        }

        public Guid WarehouseItemId { get; }
        public string Title { get; }
    }
}