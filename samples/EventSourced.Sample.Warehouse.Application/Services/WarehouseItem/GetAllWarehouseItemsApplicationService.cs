using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Persistence;
using EventSourced.Projections;
using EventSourced.Sample.Warehouse.Application.Model;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem.Projections;

namespace EventSourced.Sample.Warehouse.Application.Services.WarehouseItem
{
    internal class GetAllWarehouseItemsApplicationService : ApplicationServiceBase, IGetAllWarehouseItemsApplicationService
    {
        private readonly IManualProjectionBuilder _manualProjectionBuilder;
        private readonly IProjectionStore _projectionStore;

        public GetAllWarehouseItemsApplicationService(IManualProjectionBuilder manualProjectionBuilder, IProjectionStore projectionStore)
        {
            _manualProjectionBuilder = manualProjectionBuilder;
            _projectionStore = projectionStore;
        }

        public async Task<ICollection<WarehouseLisItemModel>> GetAllAsync(CancellationToken ct)
        {
            var allWarehouseItemsListProjection =
                await _manualProjectionBuilder.BuildProjectionAsync<AllWarehouseItemsListProjection>(ct);
            var warehouseItems = allWarehouseItemsListProjection.Items;
            return warehouseItems
                .Select(i => new WarehouseLisItemModel(i.Id, i.Title))
                .ToList();
        }

        public async Task<int> GetCountAsync(CancellationToken ct)
        {
            var warehouseItemsCountProjection = await _projectionStore.LoadProjectionAsync<WarehouseItemsCountProjection>(ct);
            return warehouseItemsCountProjection?.ExistingWarehouseItemsCount ?? 0;
        }
    }
}