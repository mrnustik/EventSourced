 using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Diagnostics.Web.Model;

namespace EventSourced.Diagnostics.Web.Services
{
    public interface IAggregateInformationService
    {
        Task<ICollection<AggregateTypesListItemModel>> GetStoredAggregateTypesAsync(CancellationToken ct);
        Task<ICollection<AggregateInstancesListItemModel>> GetStoredAggregatesOfType(Type aggregateType, CancellationToken ct);
    }
}