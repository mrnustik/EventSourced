using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Sample.Warehouse.Domain.ImportLocation.Services
{
    public interface ICreateImportLocationDomainService
    {
        Task CreateIfNotExistsAsync(CancellationToken ct);
    }
}