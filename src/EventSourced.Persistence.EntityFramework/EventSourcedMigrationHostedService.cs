using System.Threading;
using System.Threading.Tasks;
using EventSourced.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventSourced.Persistence.EntityFramework
{
    public class EventSourcedMigrationHostedService : HostedServiceBase
    {
        public EventSourcedMigrationHostedService(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory)
        {
        }

        protected override async Task StartAsync(IServiceScope serviceScope, CancellationToken cancellationToken)
        {
            var eventSourcedDbContext = serviceScope.ServiceProvider.GetRequiredService<EventSourcedDbContext>();
            await eventSourcedDbContext.Database.EnsureDeletedAsync(cancellationToken);
            await eventSourcedDbContext.Database.EnsureCreatedAsync(cancellationToken);
        }
    }
}