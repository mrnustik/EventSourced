using System;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using EventSourced.Diagnostics.Web.Helpers;
using Microsoft.AspNetCore.Http;

namespace EventSourced.Diagnostics.Web.Configuration
{
    public class EventSourcedDiagnosticsOptions
    {
        public Func<IDotvvmRequestContext, Task<bool>> Authorize { get; }

        public EventSourcedDiagnosticsOptions()
            :this(context => Task.FromResult(context.HttpContext.Request.Url.IsLoopback))
        {
        }
        
        public EventSourcedDiagnosticsOptions(Func<IDotvvmRequestContext, Task<bool>> authorize)
        {
            Authorize = authorize;
        }
    }
}