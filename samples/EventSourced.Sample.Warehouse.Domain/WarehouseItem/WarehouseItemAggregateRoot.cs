using System;
using EventSourced.Domain;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem.Events;

namespace EventSourced.Sample.Warehouse.Domain.WarehouseItem
{
    public class WarehouseItemAggregateRoot : AggregateRoot
    {
        public string Title { get; private set; } = string.Empty;

        public WarehouseItemAggregateRoot(string title)
            : base(Guid.NewGuid())
        {
            EnqueueAndApplyEvent(new WarehouseItemCreatedDomainEvent(Id, title));
        }

        public WarehouseItemAggregateRoot(Guid id)
            : base(id)
        {
        }

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException($"{nameof(title)} can not be null or empty.");
            }
            
            EnqueueAndApplyEvent(new WarehouseItemTitleUpdatedDomainEvent(Id, title));
        } 
        
        private void Apply(WarehouseItemCreatedDomainEvent @event)
        {
            Title = @event.Title;
        }

        private void Apply(WarehouseItemTitleUpdatedDomainEvent @event)
        {
            Title = @event.NewTitle;
        }
    }
}