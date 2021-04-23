using EventSourced.ExternalEvents.API.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.ExternalEvents.API.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventSourcedExternalEventsWebApi(this IServiceCollection serviceCollection,
                                                                             EventSourcedExternalWebApiOptions options)
        {
            serviceCollection.AddSingleton(options);
            serviceCollection.AddScoped<ExternalEventsHandlingMiddleware>();
            return serviceCollection;
        }
    }
}