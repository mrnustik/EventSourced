using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Persistence.InMemory
{
    public class InMemoryProjectionStore : IProjectionStore
    {
        private ConcurrentDictionary<Type, object> ProjectionsMap { get; }

        public InMemoryProjectionStore() : this(new ConcurrentDictionary<Type, object>())
        {
        }

        public InMemoryProjectionStore(ConcurrentDictionary<Type, object> projectionsMap)
        {
            ProjectionsMap = projectionsMap;
        }
        
        public Task<object?> LoadProjectionAsync(Type projectionType, CancellationToken ct)
        {
            var projection = ProjectionsMap.GetValueOrDefault(projectionType);
            return Task.FromResult(projection);
        }

        public Task StoreProjectionAsync(object projection, CancellationToken ct)
        {
            ProjectionsMap[projection.GetType()] = projection;
            return Task.CompletedTask;
        }
    }
}