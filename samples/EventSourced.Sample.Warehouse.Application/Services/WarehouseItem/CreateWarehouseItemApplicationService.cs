using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Persistence;
using EventSourced.Sample.Warehouse.Domain.WarehouseItem;

namespace EventSourced.Sample.Warehouse.Application.Services.WarehouseItem
{
    public class CreateWarehouseItemApplicationService : ApplicationServiceBase, ICreateWarehouseItemApplicationService
    {
        private readonly IRepository<WarehouseItemAggregateRoot, Guid> _repository;

        public CreateWarehouseItemApplicationService(IRepository<WarehouseItemAggregateRoot, Guid> repository)
        {
            this._repository = repository;
        }

        public Task CreateWarehouseItemAsync(string title, CancellationToken ct)
        {
            var warehouseItem = new WarehouseItemAggregateRoot(title);
            return _repository.SaveAsync(warehouseItem, ct);
        }
    }
}