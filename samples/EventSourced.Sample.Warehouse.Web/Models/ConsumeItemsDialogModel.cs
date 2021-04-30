using System;
using System.Collections.Generic;
using System.Linq;

namespace EventSourced.Sample.Warehouse.Web.Models
{
    public class ConsumeItemsDialogModel
    {
        public Guid ContainerId { get; set; }
        public ICollection<ConsumeItemsDialogItemModel> AvailableWarehouseItems { get; set; } = null!;
        public Guid SelectedWarehouseItemId { get; set; }
        public int Amount { get; set; }

        public ConsumeItemsDialogModel()
        {
        }

        public ConsumeItemsDialogModel(Guid containerId, ICollection<ConsumeItemsDialogItemModel> availableWarehouseItems, int amount)
        {
            ContainerId = containerId;
            AvailableWarehouseItems = availableWarehouseItems;
            SelectedWarehouseItemId = availableWarehouseItems.First().WarehouseItemId;
            Amount = amount;
        }
        
        public class ConsumeItemsDialogItemModel
        {
            public Guid WarehouseItemId { get; set; }
            public string WarehouseItemName { get; set; }

            public ConsumeItemsDialogItemModel()
            {
            }

            public ConsumeItemsDialogItemModel(Guid warehouseItemId, string warehouseItemName)
            {
                WarehouseItemId = warehouseItemId;
                WarehouseItemName = warehouseItemName;
            }
        }
    }
}