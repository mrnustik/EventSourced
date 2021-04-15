using System;
using System.Collections.Generic;
using System.Linq;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem.Events;

namespace EventSourced.Sample.Warehouse.Domain.WarehouseItem.Projections
{
    public class AllWarehouseItemsListProjection
    {
        public ICollection<WarehouseListItem> Items { get; } = new List<WarehouseListItem>();

        private void Apply(WarehouseItemCreatedDomainEvent @event)
        {
            Items.Add(new WarehouseListItem(@event.WarehouseItemId, @event.Title));
        }

        private void Apply(WarehouseItemTitleUpdatedDomainEvent @event)
        {
            var listItem = Items.SingleOrDefault(i => i.Id == @event.WarehouseItemId);
            if (listItem != null)
            {
                Items.Remove(listItem);
                Items.Add(new WarehouseListItem(@event.WarehouseItemId, @event.NewTitle));
            }
        }
        
        public record WarehouseListItem(Guid Id, string Title);
    }
}