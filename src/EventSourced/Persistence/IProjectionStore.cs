using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Persistence
{
    public interface IProjectionStore
    {
        public async Task<TProjection?> LoadProjectionAsync<TProjection>(CancellationToken ct) =>
            (TProjection?) await LoadProjectionAsync(typeof(TProjection), ct);
        Task<object?> LoadProjectionAsync(Type projectionType,CancellationToken ct);
        Task StoreProjectionAsync(object projection, CancellationToken ct);
    }
}