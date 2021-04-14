using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace EventSourced.Projections.Automatic
{
    public class AutomaticProjectionRebuilderHostedService : IHostedService
    {
        private readonly IAutomaticProjectionRebuilder _automaticProjectionRebuilder;

        public AutomaticProjectionRebuilderHostedService(IAutomaticProjectionRebuilder automaticProjectionRebuilder)
        {
            _automaticProjectionRebuilder = automaticProjectionRebuilder;
        }

        public Task StartAsync(CancellationToken ct)
        {
            return _automaticProjectionRebuilder.RebuildAllRegisteredAutomaticProjections(ct);
        }

        public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
    }
}