using System.Threading;
using System.Threading.Tasks;
using EventSourced.Configuration;
using EventSourced.Persistence;

namespace EventSourced.Projections.Automatic
{
    public class AutomaticProjectionRebuilder : IAutomaticProjectionRebuilder
    {
        private readonly AutomaticProjectionOptions _automaticProjectionOptions;
        private readonly IManualProjectionBuilder _manualProjectionBuilder;
        private readonly IProjectionStore _projectionStore;

        public AutomaticProjectionRebuilder(AutomaticProjectionOptions automaticProjectionOptions,
            IManualProjectionBuilder manualProjectionBuilder,
            IProjectionStore projectionStore)
        {
            _automaticProjectionOptions = automaticProjectionOptions;
            _manualProjectionBuilder = manualProjectionBuilder;
            _projectionStore = projectionStore;
        }

        public async Task RebuildAllRegisteredAutomaticProjections(CancellationToken ct)
        {
            foreach (var projectionType in _automaticProjectionOptions.RegisteredAutomaticProjections)
            {
                var projection = await _manualProjectionBuilder.BuildProjectionAsync(projectionType, ct);
                await _projectionStore.StoreProjectionAsync(projectionType, ct);
            }
        }
    }
}