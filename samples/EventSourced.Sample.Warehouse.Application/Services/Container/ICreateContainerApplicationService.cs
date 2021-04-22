using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain.Events;

namespace EventSourced.Sample.Warehouse.Application.Services.Container
{
    public interface ICreateContainerApplicationService
    {
        Task CreateContainerAsync(string identifier, CancellationToken ct);
    }
}