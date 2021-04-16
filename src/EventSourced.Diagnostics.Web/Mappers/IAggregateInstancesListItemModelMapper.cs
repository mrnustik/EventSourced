using EventSourced.Diagnostics.Web.Model.Aggregates;
using EventSourced.Domain;
using EventSourced.Domain.Events;

namespace EventSourced.Diagnostics.Web.Mappers
{
    public interface IAggregateInstancesListItemModelMapper
    {
        AggregateInstancesListItemModel MapToModel(AggregateRoot aggregateRoot, DomainEvent[] events);
    }
}