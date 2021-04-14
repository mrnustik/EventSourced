using System.Threading;
using System.Threading.Tasks;
using EventSourced.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventSourced.Projections.Automatic
{
    public class AutomaticProjectionRebuilderHostedService : HostedServiceBase
    {
        public AutomaticProjectionRebuilderHostedService(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory)
        {
        }

        protected override Task StartAsync(IServiceScope serviceScope, CancellationToken ct)
        {
            var automaticProjectionRebuilder = serviceScope.ServiceProvider.GetRequiredService<IAutomaticProjectionRebuilder>();
            return automaticProjectionRebuilder.RebuildAllRegisteredAutomaticProjections(ct);
        }
    }
}