using System;
using EventSourced.Domain;

namespace EventSourced.Sample.Warehouse.Domain.ImportLocation
{
    public class ImportedItemsValueObject : ValueObject
    {
        public Guid WarehouseItemId { get; private set; }
        public int Amount { get; private set; }

        public ImportedItemsValueObject(Guid warehouseItemId, int amount)
        {
            WarehouseItemId = warehouseItemId;
            Amount = amount;
        }
    }
}