using EventSourced.Configuration;
using EventSourced.Sample.Warehouse.Domain.ImportLocation.Projections;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem.Projections;

namespace EventSourced.Sample.Warehouse.Domain.Configuration
{
    public static class EventSourcedOptionsExtension
    {
        public static EventSourcedOptions ConfigureDomainObjects(this EventSourcedOptions options)
        {
            return options.RegisterAutomaticProjection<ImportLocationProjection>()
                          .RegisterAutomaticProjection<AllWarehouseItemsListProjection>()
                          .RegisterAutomaticProjection<WarehouseItemsCountProjection>()
                          .RegisterAutomaticAggregateProjection<WarehouseItemDetailProjection, WarehouseItemAggregateRoot>();
        }
    }
}