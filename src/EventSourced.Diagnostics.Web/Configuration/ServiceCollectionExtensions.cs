using EventSourced.Diagnostics.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Diagnostics.Web.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventSourcedDiagnostics(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDotVVM<DotvvmStartup>();
            serviceCollection.AddTransient<IAggregateInformationService, AggregateInformationService>();
            return serviceCollection;
        }
    }
}