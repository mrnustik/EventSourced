﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Persistence
{
    public interface IProjectionStore
    {
        public async Task<TProjection?> LoadProjectionAsync<TProjection>(CancellationToken ct) =>
            (TProjection?) await LoadProjectionAsync(typeof(TProjection), ct);

        Task<object?> LoadProjectionAsync(Type projectionType, CancellationToken ct);

        public async Task<TAggregateProjection?> LoadAggregateProjectionAsync<TAggregateProjection, TAggregateRoot>(
            Guid aggregateRootId,
            CancellationToken ct)
            => (TAggregateProjection?) await LoadAggregateProjectionAsync(typeof(TAggregateProjection), aggregateRootId, ct);

        Task<object?> LoadAggregateProjectionAsync(
            Type aggregateRootProjection,
            Guid aggregateRootId,
            CancellationToken ct);

        Task StoreProjectionAsync(object projection, CancellationToken ct);
        Task StoreAggregateProjectionAsync(Guid streamId, object aggregateProjection, CancellationToken ct);
    }
}