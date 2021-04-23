using System;

namespace EventSourced.Sample.Warehouse.Application.Model
{
    public record ImportLocationContentListItemModel(Guid WarehouseItemId, string WarehouseItemName, int Amount);
}