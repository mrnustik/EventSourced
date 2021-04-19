using System;
using System.Collections.Generic;
using EventSourced.Sample.Warehouse.Domain.Container.Events;

namespace EventSourced.Sample.Warehouse.Domain.Container.Projections
{
    public class AllContainersProjection
    {
        public ICollection<ContainerListItem> Containers { get; private set; } = new List<ContainerListItem>();

        private void Apply(ContainerCreatedDomainEvent domainEvent)
        {
            Containers.Add(new ContainerListItem(domainEvent.Id, domainEvent.Identifier));
        }
        
        public record ContainerListItem(Guid ContainerId, string Identifier);
    }
}