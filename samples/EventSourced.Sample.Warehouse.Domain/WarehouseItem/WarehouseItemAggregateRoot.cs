using System;
using EventSourced.Domain;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem.Events;

namespace EventSourced.Sample.Warehouse.Domain.WarehouseItem
{
    public class WarehouseItemAggregateRoot : AggregateRoot<Guid>
    {
        public WarehouseItemAggregateRoot() : base(Guid.NewGuid())
        {
        }

        public WarehouseItemAggregateRoot(string title) : base(Guid.NewGuid())
        {
            EnqueueAndApplyEvent(new WarehouseItemCreatedDomainEvent(Id, title));
        }

        public WarehouseItemAggregateRoot(Guid id) : base(id)
        {
        }

        public string Title { get; private set; }

        public void Apply(WarehouseItemCreatedDomainEvent @event)
        {
            Title = @event.Title;
        }
    }
}