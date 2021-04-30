﻿using System;
using System.Collections.Generic;

namespace EventSourced.Sample.Warehouse.Application.Model
{
    public record ContainerDetailModel(Guid Id,
                                       string Identifier,
                                       ICollection<ContainerContentListItemModel> ContainerContents);

    public record ContainerContentListItemModel(Guid WarehouseItemId, string WarehouseItemName, int Amount);
}