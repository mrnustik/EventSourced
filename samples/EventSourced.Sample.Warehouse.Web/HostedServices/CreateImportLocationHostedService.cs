using System.Threading;
using System.Threading.Tasks;
using EventSourced.Helpers;
using EventSourced.Sample.Warehouse.Application.Services.ImportLocation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventSourced.Sample.Warehouse.Web.HostedServices
{
    public class CreateImportLocationHostedService : HostedServiceBase
    {
        public CreateImportLocationHostedService(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory)
        {
        }

        protected override Task StartAsync(IServiceScope serviceScope, CancellationToken cancellationToken)
        {
            var createImportLocationApplicationService = serviceScope.ServiceProvider.GetRequiredService<ICreateImportLocationApplicationService>();
            return createImportLocationApplicationService.CreateImportLocationAsync(cancellationToken);
        }
    }
}