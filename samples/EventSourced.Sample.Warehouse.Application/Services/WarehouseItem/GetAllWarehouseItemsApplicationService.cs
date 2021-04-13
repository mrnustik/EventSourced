using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Projections;
using EventSourced.Sample.Warehouse.Application.Model;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem.Projections;

namespace EventSourced.Sample.Warehouse.Application.Services.WarehouseItem
{
    internal class GetAllWarehouseItemsApplicationService : ApplicationServiceBase, IGetAllWarehouseItemsApplicationService
    {
        private readonly IManualProjectionBuilder _manualProjectionBuilder;

        public GetAllWarehouseItemsApplicationService(IManualProjectionBuilder manualProjectionBuilder)
        {
            _manualProjectionBuilder = manualProjectionBuilder;
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
    }
}