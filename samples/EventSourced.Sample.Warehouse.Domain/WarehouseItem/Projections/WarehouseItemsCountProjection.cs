using EventSourced.Sample.Warehouse.Domain.WarehouseItem.Events;

namespace EventSourced.Sample.Warehouse.Domain.WarehouseItem.Projections
{
    public class WarehouseItemsCountProjection
    {
        public int ExistingWarehouseItemsCount { get; private set; }

        private void Apply(WarehouseItemCreatedDomainEvent _)
        {
            ExistingWarehouseItemsCount++;
        }
    }
}