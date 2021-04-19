using System;

namespace EventSourced.Sample.Warehouse.Application.Model
{
    public record ContainerListItemModel(Guid ContainerId, string Identifier);
}