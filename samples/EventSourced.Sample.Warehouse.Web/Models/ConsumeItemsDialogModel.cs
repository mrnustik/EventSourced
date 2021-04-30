using System;
using System.Collections.Generic;
using System.Linq;

namespace EventSourced.Sample.Warehouse.Web.Models
{
    public class ConsumeItemsDialogModel
    {
        public Guid ContainerId { get; set; }
        public ICollection<DialogWarehouseItemModel> AvailableWarehouseItems { get; set; } = null!;
        public Guid SelectedWarehouseItemId { get; set; }
        public int Amount { get; set; }

        public ConsumeItemsDialogModel()
        {
        }

        public ConsumeItemsDialogModel(Guid containerId, ICollection<DialogWarehouseItemModel> availableWarehouseItems)
        {
            ContainerId = containerId;
            AvailableWarehouseItems = availableWarehouseItems;
            SelectedWarehouseItemId = availableWarehouseItems.First()
                                                             .WarehouseItemId;
        }
    }
}