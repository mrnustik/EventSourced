using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;

namespace EventSourced.Persistence
{
    public interface IRepository<TAggregateRoot> 
        where TAggregateRoot : AggregateRoot, new()
    {
        Task SaveAsync(TAggregateRoot aggregateRoot, CancellationToken ct);
        Task<TAggregateRoot> GetByIdAsync(Guid id, CancellationToken ct);
    }
}