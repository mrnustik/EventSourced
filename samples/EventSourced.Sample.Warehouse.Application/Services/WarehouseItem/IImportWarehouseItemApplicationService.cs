using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Sample.Warehouse.Application.Services.WarehouseItem
{
    public interface IImportWarehouseItemApplicationService
    {
        Task ImportWarehouseItemAsync(Guid warehouseItemId, int amount, CancellationToken ct);
    }
}