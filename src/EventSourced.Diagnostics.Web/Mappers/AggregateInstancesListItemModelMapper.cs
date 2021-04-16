using EventSourced.Diagnostics.Web.Model.Aggregates;
using EventSourced.Domain;
using EventSourced.Domain.Events;
using Newtonsoft.Json;

namespace EventSourced.Diagnostics.Web.Mappers
{
    class AggregateInstancesListItemModelMapper : IAggregateInstancesListItemModelMapper
    {
        private JsonSerializerSettings EventSerializationSettings => new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects
        };
        
        public AggregateInstancesListItemModel MapToModel(AggregateRoot aggregateRoot, DomainEvent[] events)
        {
            return new AggregateInstancesListItemModel(aggregateRoot.Id,
                                                       aggregateRoot.Version,
                                                       JsonConvert.SerializeObject(aggregateRoot),
                                                       JsonConvert.SerializeObject(events, EventSerializationSettings));
        }
    }
}