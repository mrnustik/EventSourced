using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Persistence;
using EventSourced.Sample.Warehouse.Application.Model;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem;

namespace EventSourced.Sample.Warehouse.Application.Services.WarehouseItem
{
    class GetAllWarehouseItemsApplicationService : ApplicationServiceBase, IGetAllWarehouseItemsApplicationService
    {
        private readonly IRepository<WarehouseItemAggregateRoot, Guid> _repository;

        public GetAllWarehouseItemsApplicationService(IRepository<WarehouseItemAggregateRoot, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<ICollection<WarehouseLisItemModel>> GetAllAsync(CancellationToken ct)
        {
            var warehouseItems = await _repository.GetAllAsync(ct);
            return warehouseItems
                .Select(i => new WarehouseLisItemModel(i.Id, i.Title))
                .ToList();
        }
    }
}