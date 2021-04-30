using System;
using System.Collections.Generic;
using System.Linq;
using DotVVM.Framework.ViewModel;
using EventSourced.Sample.Warehouse.Application.Model;

namespace EventSourced.Sample.Warehouse.Web.Models
{
    public class MoveItemsDialogModel
    {
        public Guid SourceContainerId { get; set; }
        [Bind(Direction.ServerToClient)]
        public ICollection<DialogWarehouseItemModel> AvailableWarehouseItems { get; set; } = null!;
        public Guid SelectedWarehouseItemId { get; set; }
        [Bind(Direction.ServerToClient)]
        public ICollection<ContainerListItemModel> AvailableContainers { get; set; } = null!;
        public Guid DestinationContainerId { get; set; }
        public int Amount { get; set; }

        public MoveItemsDialogModel()
        {
        }

        public MoveItemsDialogModel(Guid sourceContainerId,
                                    ICollection<DialogWarehouseItemModel> availableWarehouseItems,
                                    ICollection<ContainerListItemModel> availableContainers)
        {
            SourceContainerId = sourceContainerId;
            AvailableWarehouseItems = availableWarehouseItems;
            SelectedWarehouseItemId = AvailableWarehouseItems.First().WarehouseItemId;
            AvailableContainers = availableContainers;
            DestinationContainerId = AvailableContainers.First().ContainerId;
        }
    }
}