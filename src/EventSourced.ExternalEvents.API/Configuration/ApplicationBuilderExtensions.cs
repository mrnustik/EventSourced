using EventSourced.ExternalEvents.API.Middleware;
using Microsoft.AspNetCore.Builder;

namespace EventSourced.ExternalEvents.API.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseEventSourcedExternalEventsWebApi(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<ExternalEventsHandlingMiddleware>();
        } 
    }
}