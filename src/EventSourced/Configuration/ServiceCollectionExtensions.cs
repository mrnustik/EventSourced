using System;
using EventSourced.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEventSourced(this IServiceCollection serviceCollection, Action<EventSourcedOptions> optionsConfiguration)
        {
            serviceCollection.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));

            var options = new EventSourcedOptions(serviceCollection);
            optionsConfiguration(options);
            serviceCollection.AddSingleton(options);
        }
    }
}