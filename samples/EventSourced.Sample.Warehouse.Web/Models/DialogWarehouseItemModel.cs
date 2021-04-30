using System;

namespace EventSourced.Sample.Warehouse.Web.Models
{
    public class DialogWarehouseItemModel
    {
        public Guid WarehouseItemId { get; set; }
        public string WarehouseItemName { get; set; }

        public DialogWarehouseItemModel()
        {
        }

        public DialogWarehouseItemModel(Guid warehouseItemId, string warehouseItemName)
        {
            WarehouseItemId = warehouseItemId;
            WarehouseItemName = warehouseItemName;
        }
    }
}