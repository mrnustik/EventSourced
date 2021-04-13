﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;

namespace EventSourced.Persistence
{
    public interface IRepository<TAggregateRoot, in TAggregateRootId>
        where TAggregateRoot : AggregateRoot<TAggregateRootId>
        where TAggregateRootId : notnull
    {
        Task SaveAsync(TAggregateRoot aggregateRoot, CancellationToken ct);
        Task<TAggregateRoot> GetByIdAsync(TAggregateRootId id, CancellationToken ct);
        Task<ICollection<TAggregateRoot>> GetAllAsync(CancellationToken ct);
    }
}