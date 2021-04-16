using System.Threading;
using System.Threading.Tasks;
using EventSourced.Sample.Warehouse.Domain.ImportLocation.Services;

namespace EventSourced.Sample.Warehouse.Application.Services.ImportLocation
{
    class CreateImportLocationApplicationService : ApplicationServiceBase, ICreateImportLocationApplicationService
    {
        private readonly ICreateImportLocationDomainService _createImportLocationDomainService;

        public CreateImportLocationApplicationService(ICreateImportLocationDomainService createImportLocationDomainService)
        {
            _createImportLocationDomainService = createImportLocationDomainService;
        }

        public Task CreateImportLocationAsync(CancellationToken ct)
        {
            return _createImportLocationDomainService.CreateIfNotExistsAsync(ct);
        }
    }
}