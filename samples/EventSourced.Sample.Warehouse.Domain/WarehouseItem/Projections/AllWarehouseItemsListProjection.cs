using System;
using System.Collections.Generic;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem.Events;

namespace EventSourced.Sample.Warehouse.Domain.WarehouseItem.Projections
{
    public class AllWarehouseItemsListProjection
    {
        public record WarehouseListItem(Guid Id, string Title);
        public ICollection<WarehouseListItem> Items { get; } = new List<WarehouseListItem>();

        public void Apply(WarehouseItemCreatedDomainEvent @event)
        {
            Items.Add(new WarehouseListItem(@event.WarehouseItemId, @event.Title));
        } 
    }
}