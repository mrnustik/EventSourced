using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Diagnostics.Web.Model.Projections;

namespace EventSourced.Diagnostics.Web.Services
{
    public interface IProjectionInformationService
    {
        Task<ICollection<TypeBasedProjectionModel>> GetTypeBasedProjectionsAsync(CancellationToken ct);
        Task<ICollection<AggregateBasedProjectionTypeModel>> GetAllAggregateProjectionTypesAsync(CancellationToken ct);
    }
}