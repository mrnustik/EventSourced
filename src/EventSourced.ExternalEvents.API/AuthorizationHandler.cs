using System.Threading;
using System.Threading.Tasks;
using EventSourced.ExternalEvents.API.Configuration;
using Microsoft.AspNetCore.Http;

namespace EventSourced.ExternalEvents.API
{
    class AuthorizationHandler : IAuthorizationHandler
    {
        private readonly EventSourcedExternalWebApiOptions _externalWebApiOptions;

        public AuthorizationHandler(EventSourcedExternalWebApiOptions externalWebApiOptions)
        {
            _externalWebApiOptions = externalWebApiOptions;
        }

        public Task<bool> AuthorizeAsync(HttpContext context, CancellationToken ct)
        {
            return _externalWebApiOptions.Authorize(context);
        }
    }
}