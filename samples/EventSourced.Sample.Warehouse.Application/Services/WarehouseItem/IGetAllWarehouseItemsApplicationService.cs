using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Sample.Warehouse.Application.Model;

namespace EventSourced.Sample.Warehouse.Application.Services.WarehouseItem
{
    public interface IGetAllWarehouseItemsApplicationService
    {
        Task<ICollection<WarehouseLisItemModel>> GetAllAsync(CancellationToken ct);
        Task<int> GetCountAsync(CancellationToken ct);
    }
}