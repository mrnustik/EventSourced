using System;
using System.Collections.Generic;
using System.Linq;
using EventSourced.Domain;
using EventSourced.Sample.Warehouse.Domain.Exceptions;
using EventSourced.Sample.Warehouse.Domain.ImportLocation.Events;

namespace EventSourced.Sample.Warehouse.Domain.ImportLocation
{
    public class ImportLocationAggregateRoot : AggregateRoot
    {
        public ICollection<ImportedItemsValueObject> ImportedItems { get; private set; } = new List<ImportedItemsValueObject>();

        internal ImportLocationAggregateRoot() : base(Guid.NewGuid())
        {
            EnqueueAndApplyEvent(new ImportLocationCreatedDomainEvent(Id));
        }
        
        public ImportLocationAggregateRoot(Guid id)
            : base(id)
        {
        }

        public void ImportWarehouseItem(Guid warehouseItemId, int amount)
        {
            if (amount <= 0)
            {
                throw new BusinessRuleException("Number of imported items has to be greater than zero.");
            }

            EnqueueAndApplyEvent(new NewItemsImportedDomainEvent(warehouseItemId, amount));
        }

        private void Apply(ImportLocationCreatedDomainEvent domainEvent)
        {
        }
        
        private void Apply(NewItemsImportedDomainEvent domainEvent)
        {
            var existingValueObject = ImportedItems.SingleOrDefault(i => i.WarehouseItemId == domainEvent.WarehouseItemId);
            if (existingValueObject != null)
            {
                ImportedItems.Remove(existingValueObject);
            }
            ImportedItems.Add(new ImportedItemsValueObject(domainEvent.WarehouseItemId, domainEvent.Amount + existingValueObject?.Amount ?? 0));
        }
    }
}