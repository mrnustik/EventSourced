using System;
using EventSourced.Domain;

namespace EventSourced.Sample.Warehouse.Domain.Container
{
    public class WarehouseItemInContainerValueObject : ValueObject
    {
        public Guid WarehouseItemId { get; }
        public int Amount { get;}

        public WarehouseItemInContainerValueObject(Guid warehouseItemId, int amount)
        {
            WarehouseItemId = warehouseItemId;
            Amount = amount;
        }
    }
}