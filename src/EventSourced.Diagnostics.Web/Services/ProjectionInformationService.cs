using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ICollection<AggregateBasedProjectionTypeModel>> GetAllAggregateProjectionTypesAsync(CancellationToken ct)
        {
            var projections = await _projectionStore.LoadAllAggregateProjectionsAsync(ct);
            return projections.SelectMany(p => p.Value)
                              .Select(p => p.GetType())
                              .Distinct()
                              .Select(p => new AggregateBasedProjectionTypeModel(p))
                              .ToList();
        }

        public async Task<ICollection<AggregateProjectionValueModel>> GetAggregateProjectionsOfTypeAsync(Type aggregateProjectionType, CancellationToken ct)
        {
            var alProjections = await _projectionStore.LoadAllAggregateProjectionsAsync(ct);
            var aggregateProjections = new List<AggregateProjectionValueModel>();
            foreach (var (aggregateId, projections) in alProjections.Where(v => v.Value.Any(v => v.GetType() == aggregateProjectionType)))
            {
                foreach (object projection in projections)
                {
                    var type = projection.GetType();
                    if(type != aggregateProjectionType) continue;
                    aggregateProjections.Add(new AggregateProjectionValueModel(aggregateId, JsonConvert.SerializeObject(projection)));
                }
            }
            return aggregateProjections;
        }
    }
}