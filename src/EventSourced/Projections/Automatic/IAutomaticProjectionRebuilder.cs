using System.Threading;
using System.Threading.Tasks;

namespace EventSourced.Projections.Automatic
{
    public interface IAutomaticProjectionRebuilder
    {
        Task RebuildAllRegisteredAutomaticProjections(CancellationToken ct);
    }
}