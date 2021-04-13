using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Projections
{
    public interface IManualProjectionBuilder
    {
        Task<TProjection> BuildProjectionAsync<TProjection>(CancellationToken ct)
            where TProjection : new();
    }
}