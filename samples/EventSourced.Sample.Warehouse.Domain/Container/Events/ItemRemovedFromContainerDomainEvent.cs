using System;
using EventSourced.Domain.Events;

namespace EventSourced.Sample.Warehouse.Domain.Container.Events
{
    public class ItemRemovedFromContainerDomainEvent : DomainEvent
    {
        public Guid WarehouseItemId { get; }
        public int Amount { get;}
        
        public ItemRemovedFromContainerDomainEvent(Guid warehouseItemId, int amount)
        {
            WarehouseItemId = warehouseItemId;
            Amount = amount;
        }
    }
}