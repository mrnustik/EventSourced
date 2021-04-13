using System;
using EventSourced.Persistence;
using EventSourced.Projections;
using EventSourced.Projections.Automatic;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEventSourced(this IServiceCollection serviceCollection, Action<EventSourcedOptions> optionsConfiguration)
        {
            serviceCollection.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
            serviceCollection.AddTransient<IManualProjectionBuilder, ManualProjectionBuilder>();

            var options = new EventSourcedOptions(serviceCollection);
            optionsConfiguration(options);

            var automaticProjectionsEventMapper = new AutomaticProjectionsEventMapper(options.AutomaticProjectionOptions);
            serviceCollection.AddSingleton(automaticProjectionsEventMapper);
        }
    }
}