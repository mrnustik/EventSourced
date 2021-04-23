using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Persistence;
using EventSourced.Projections;
using EventSourced.Sample.Warehouse.Application.Model;
using EventSourced.Sample.Warehouse.Domain.Exceptions;
using EventSourced.Sample.Warehouse.Domain.ImportLocation;
using EventSourced.Sample.Warehouse.Domain.ImportLocation.Projections;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem.Projections;

namespace EventSourced.Sample.Warehouse.Application.Services.ImportLocation
{
    class GetImportLocationDataApplicationService : ApplicationServiceBase, IGetImportLocationDataApplicationService
    {
        private readonly IProjectionStore _projectionStore;
        private readonly IManualProjectionBuilder _manualProjectionBuilder;

        public GetImportLocationDataApplicationService(IProjectionStore projectionStore,
                                                       IManualProjectionBuilder manualProjectionBuilder)
        {
            _projectionStore = projectionStore;
            _manualProjectionBuilder = manualProjectionBuilder;
        }

        public async Task<ICollection<ImportLocationContentListItemModel>> GetImportLocationContentAsync(CancellationToken ct)
        {
            var importLocationProjection = await _projectionStore.LoadProjectionAsync<ImportLocationProjection>(ct);
            if (importLocationProjection == null)
            {
                throw new BusinessRuleException("Import location was not found");
            }
            var importLocationContentProjection = await _manualProjectionBuilder.BuildAggregateProjection<ImportLocationContentProjection, ImportLocationAggregateRoot>(importLocationProjection.ImportLocationId, ct);
            var allWarehouseItemProjection = await _projectionStore.LoadProjectionAsync<AllWarehouseItemsListProjection>(ct);
            return importLocationContentProjection.ImportedItems.Select(i => new ImportLocationContentListItemModel(i.WarehouseItemId,
                                                                            allWarehouseItemProjection!.Items
                                                                                .Single(x => i.WarehouseItemId == x.Id)
                                                                                .Title,
                                                                            i.Amount))
                                                  .ToList();
        }
    }
}