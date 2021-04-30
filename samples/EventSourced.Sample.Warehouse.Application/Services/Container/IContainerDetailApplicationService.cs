using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Persistence;
using EventSourced.Projections;
using EventSourced.Sample.Warehouse.Application.Model;
using EventSourced.Sample.Warehouse.Domain.Container;
using EventSourced.Sample.Warehouse.Domain.Container.Projections;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem.Projections;

namespace EventSourced.Sample.Warehouse.Application.Services.Container
{
    public interface IContainerDetailApplicationService
    {
        Task<ContainerDetailModel> GetContainerDetailAsync(Guid containerId, CancellationToken ct);
    }

    class ContainerDetailApplicationService : ApplicationServiceBase, IContainerDetailApplicationService
    {
        private readonly IManualProjectionBuilder _manualProjectionBuilder;
        private readonly IProjectionStore _projectionStore;

        public ContainerDetailApplicationService(IManualProjectionBuilder manualProjectionBuilder, IProjectionStore projectionStore)
        {
            _manualProjectionBuilder = manualProjectionBuilder;
            _projectionStore = projectionStore;
        }
        
        public async Task<ContainerDetailModel> GetContainerDetailAsync(Guid containerId, CancellationToken ct)
        {
            var containerDetailProjection = await _manualProjectionBuilder.BuildAggregateProjection<ContainerDetailProjection, ContainerAggregateRoot>(containerId, ct);
            var allWarehouseItems = await _projectionStore.LoadProjectionAsync<AllWarehouseItemsListProjection>(ct);

            return new ContainerDetailModel(containerId,
                                            containerDetailProjection.Identifier,
                                            containerDetailProjection.ContainedWarehouseItems
                                                                     .Select(i => new ContainerContentListItemModel(
                                                                                 i.Key,
                                                                                 allWarehouseItems!.GetWarehouseItemTitle(i.Key),
                                                                                 i.Value))
                                                                     .ToList());
        }
    }
}