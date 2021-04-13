using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Sample.Warehouse.Application.Services.WarehouseItem
{
    public interface ICreateWarehouseItemApplicationService
    {
        Task CreateWarehouseItemAsync(string title, CancellationToken ct);
    }
}