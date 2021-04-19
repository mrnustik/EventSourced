using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Persistence;
using EventSourced.Sample.Warehouse.Application.Model;
using EventSourced.Sample.Warehouse.Domain.Container.Projections;

namespace EventSourced.Sample.Warehouse.Application.Services.Container
{
    class GetAllContainersApplicationService : ApplicationServiceBase, IGetAllContainersApplicationService
    {
        private readonly IProjectionStore _projectionStore;

        public GetAllContainersApplicationService(IProjectionStore projectionStore)
        {
            _projectionStore = projectionStore;
        }

        public async Task<ICollection<ContainerListItemModel>> GetAllAsync(CancellationToken ct)
        {
            var projection = await _projectionStore.LoadProjectionAsync<AllContainersProjection>(ct);
            if (projection == null)
            {
                return new List<ContainerListItemModel>();
            }
            return projection.Containers.Select(c => new ContainerListItemModel(c.ContainerId, c.Identifier))
                             .ToList();
        }
    }
}