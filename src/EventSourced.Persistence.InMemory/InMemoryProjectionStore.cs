﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Persistence.InMemory
{
    public class InMemoryProjectionStore : IProjectionStore
    {
        private ConcurrentDictionary<Type, object> ProjectionsMap { get; }
        private ConcurrentDictionary<Type, ConcurrentDictionary<Guid, object>> AggregateProjectionsMap { get; }

        public InMemoryProjectionStore() : this(new ConcurrentDictionary<Type, object>())
        {
        }

        public InMemoryProjectionStore(ConcurrentDictionary<Type, object> projectionsMap)
        {
            ProjectionsMap = projectionsMap;
            AggregateProjectionsMap = new ConcurrentDictionary<Type, ConcurrentDictionary<Guid, object>>();
        }
        
        public Task<object?> LoadProjectionAsync(Type projectionType, CancellationToken ct)
        {
            var projection = ProjectionsMap.GetValueOrDefault(projectionType);
            return Task.FromResult(projection);
        }

        public Task<object?> LoadAggregateProjectionAsync(Type aggregateRootProjection, Guid aggregateRootId, CancellationToken ct)
        {
            if (AggregateProjectionsMap.TryGetValue(aggregateRootProjection, out var projectionByIdDictionary))
            {
                if (projectionByIdDictionary.TryGetValue(aggregateRootId, out var projection))
                {
                    return Task.FromResult<object?>(projection);
                }
            }
            return Task.FromResult<object?>(null);
        }

        public Task StoreProjectionAsync(object projection, CancellationToken ct)
        {
            ProjectionsMap[projection.GetType()] = projection;
            return Task.CompletedTask;
        }

        public Task StoreAggregateProjectionAsync(Guid streamId, object aggregateProjection, CancellationToken ct)
        {
            var projectionType = aggregateProjection.GetType();
            if (!AggregateProjectionsMap.ContainsKey(projectionType))
            {
                AggregateProjectionsMap[projectionType] = new ConcurrentDictionary<Guid, object>();
            }

            AggregateProjectionsMap[projectionType][streamId] = aggregateProjection;
            return Task.CompletedTask;
        }
    }
}