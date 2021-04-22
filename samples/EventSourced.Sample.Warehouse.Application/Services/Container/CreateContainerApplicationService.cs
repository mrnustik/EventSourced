using System.Threading;
using System.Threading.Tasks;
using EventSourced.Persistence;
using EventSourced.Sample.Warehouse.Domain.Container;

namespace EventSourced.Sample.Warehouse.Application.Services.Container
{
    class CreateContainerApplicationService : ApplicationServiceBase, ICreateContainerApplicationService
    {
        private readonly IRepository<ContainerAggregateRoot> _containerRepository;

        public CreateContainerApplicationService(IRepository<ContainerAggregateRoot> containerRepository)
        {
            _containerRepository = containerRepository;
        }

        public Task CreateContainerAsync(string identifier, CancellationToken ct)
        {
            var container = new ContainerAggregateRoot(identifier);
            return _containerRepository.SaveAsync(container, ct);
        }
    }
}