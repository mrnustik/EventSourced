using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EventSourced.ExternalEvents.API
{
    public interface IAuthorizationHandler
    {
        Task<bool> AuthorizeAsync(HttpContext context, CancellationToken ct);
    }
}