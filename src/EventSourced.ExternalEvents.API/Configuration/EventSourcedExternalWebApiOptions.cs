using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EventSourced.ExternalEvents.API.Configuration
{
    public class EventSourcedExternalWebApiOptions
    {
        public string ExternalEventsEndpoint { get; }
        public Func<HttpContext, Task<bool>> Authorize { get; }
        
        public EventSourcedExternalWebApiOptions(string externalEventsEndpoint) 
            : this(externalEventsEndpoint, request => Task.FromResult(true))
        {
        }

        public EventSourcedExternalWebApiOptions(string externalEventsEndpoint, Func<HttpContext, Task<bool>> authorize)
        {
            ExternalEventsEndpoint = externalEventsEndpoint;
            Authorize = authorize;
        }
    }
}