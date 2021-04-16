using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Sample.Warehouse.Application.Services.ImportLocation
{
    public interface ICreateImportLocationApplicationService
    {
        Task CreateImportLocationAsync(CancellationToken ct);
    }
}