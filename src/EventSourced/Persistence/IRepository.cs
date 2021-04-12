using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;

namespace EventSourced.Persistence
{
    public interface IRepository<TAggregateRoot, in TAggregateRootId> 
        where TAggregateRoot : AggregateRoot<TAggregateRootId>, new()
        where TAggregateRootId : notnull
    {
        Task SaveAsync(TAggregateRoot aggregateRoot, CancellationToken ct);
        Task<TAggregateRoot> GetByIdAsync(TAggregateRootId id, CancellationToken ct);
    }
}