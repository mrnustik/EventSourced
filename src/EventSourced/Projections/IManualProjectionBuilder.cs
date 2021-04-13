using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;

namespace EventSourced.Projections
{
    public interface IManualProjectionBuilder
    {
        Task<TProjection> BuildProjectionAsync<TProjection>(CancellationToken ct)
            where TProjection : new();

        Task<TAggregateProjection> BuildAggregateProjection<TAggregateProjection, TAggregateRoot, TAggregateRootId>(TAggregateRootId id, CancellationToken ct)
            where TAggregateProjection : new()
            where TAggregateRootId : notnull
            where TAggregateRoot : AggregateRoot<TAggregateRootId>;
    }
}