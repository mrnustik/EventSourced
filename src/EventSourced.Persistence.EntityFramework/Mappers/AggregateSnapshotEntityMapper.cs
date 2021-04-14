using System;
using EventSourced.Domain;
using EventSourced.Domain.Snapshosts;
using EventSourced.Persistence.EntityFramework.Entities;
using EventSourced.Persistence.EntityFramework.Helpers;
using JsonNet.ContractResolvers;
using Newtonsoft.Json;

namespace EventSourced.Persistence.EntityFramework.Mappers
{
    public class AggregateSnapshotEntityMapper : IAggregateSnapshotEntityMapper
    {
        private readonly ITypeSerializer _typeSerializer;

        private JsonSerializerSettings SerializerSettings = new()
        {
            ContractResolver = new PrivateSetterAndCtorContractResolver()
        };
        
        public AggregateSnapshotEntityMapper(ITypeSerializer typeSerializer)
        {
            _typeSerializer = typeSerializer;
        }

        public AggregateSnapshotEntity MapToEntity<TAggregateRoot>(AggregateSnapshot<TAggregateRoot> aggregateSnapshot)
            where TAggregateRoot : AggregateRoot
        {
            var serializedAggregateRootType = _typeSerializer.SerializeType(typeof(TAggregateRoot));

            return new AggregateSnapshotEntity
            {
                Id = aggregateSnapshot.Id,
                Version = aggregateSnapshot.Version,
                AggregateRootType = serializedAggregateRootType,
                SerializedAggregateState = JsonConvert.SerializeObject(aggregateSnapshot.AggregateState, SerializerSettings)
            };
        }

        public TAggregateRoot MapToAggregateRoot<TAggregateRoot>(AggregateSnapshotEntity entity)
            where TAggregateRoot : AggregateRoot
        {
            var aggregateRoot = (TAggregateRoot) Activator.CreateInstance(typeof(TAggregateRoot), entity.Id)!;
            JsonConvert.PopulateObject(entity.SerializedAggregateState, aggregateRoot, SerializerSettings);
            return aggregateRoot;
        }
    }
}