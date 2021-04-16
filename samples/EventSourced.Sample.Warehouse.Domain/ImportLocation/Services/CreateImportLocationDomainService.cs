using System.Threading;
using System.Threading.Tasks;
using EventSourced.Persistence;
using EventSourced.Sample.Warehouse.Domain.Exceptions;

namespace EventSourced.Sample.Warehouse.Domain.ImportLocation.Services
{
    public class CreateImportLocationDomainService : ICreateImportLocationDomainService
    {
        private readonly IRepository<ImportLocationAggregateRoot> _repository;

        public CreateImportLocationDomainService(IRepository<ImportLocationAggregateRoot> repository)
        {
            _repository = repository;
        }

        public async Task CreateIfNotExistsAsync(CancellationToken ct)
        {
            var existingLocations = await _repository.GetAllAsync(ct);
            if (existingLocations.Count == 0)
            {
                var location = new ImportLocationAggregateRoot();
                await _repository.SaveAsync(location, ct);
            }
            else if(existingLocations.Count > 1)
            {
                throw new BusinessRuleException("More than one import location already exists.");
            }
        }
    }
}