using Microsoft.AspNetCore.Builder;

namespace EventSourced.Diagnostics.Web.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseEventSourcedDiagnostics(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseDotVVM<DotvvmStartup>();
            return applicationBuilder;
        }   
    }
}