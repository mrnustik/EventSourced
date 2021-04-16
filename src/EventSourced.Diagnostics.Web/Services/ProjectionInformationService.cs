using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Diagnostics.Web.Model.Projections;
using EventSourced.Persistence;
using Newtonsoft.Json;

namespace EventSourced.Diagnostics.Web.Services
{
    public class ProjectionInformationService : IProjectionInformationService
    {
        private readonly IProjectionStore _projectionStore;

        public ProjectionInformationService(IProjectionStore projectionStore)
        {
            _projectionStore = projectionStore;
        }

        public async Task<ICollection<TypeBasedProjectionModel>> GetTypeBasedProjectionsAsync(CancellationToken ct)
        {
            var projections = await _projectionStore.LoadAllProjectionsAsync(ct);
            return projections.Select(p => new TypeBasedProjectionModel(p.GetType(), JsonConvert.SerializeObject(p)))
                              .ToList();
        }
    }
}