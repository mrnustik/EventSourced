using System;
using EventSourced.Domain.Events;
using EventSourced.Persistence;
using EventSourced.Persistence.Null;
using EventSourced.Projections;
using EventSourced.Projections.Automatic;
using EventSourced.Snapshots;
using EventSourced.Snapshots.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEventSourced(this IServiceCollection serviceCollection, Action<EventSourcedOptions> optionsConfiguration)
        {
            serviceCollection.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            serviceCollection.AddTransient<IManualProjectionBuilder, ManualProjectionBuilder>();
            serviceCollection.AddTransient<IDomainEventHandler, AutomaticProjectionDomainEventHandler>();
            serviceCollection.AddTransient<IDomainEventHandler, AutomaticAggregateProjectionDomainEventHandler>();
            serviceCollection.AddTransient<IAutomaticProjectionRebuilder, AutomaticProjectionRebuilder>();
            serviceCollection.AddTransient(typeof(ISnapshotStore<>), typeof(NullSnapshotStore<>));
            serviceCollection.AddTransient<ISnapshotCreationStrategy, NullSnapshotCreationStrategy>();
            
            var options = new EventSourcedOptions(serviceCollection);
            optionsConfiguration(options);
            serviceCollection.AddSingleton(options.AutomaticProjectionOptions);

            var automaticProjectionsEventMapper = new AutomaticProjectionsEventMapper(options.AutomaticProjectionOptions);
            automaticProjectionsEventMapper.Initialize();
            serviceCollection.AddSingleton<IAutomaticProjectionsEventMapper>(automaticProjectionsEventMapper);

            if (options.AutomaticProjectionOptions.RebuildAutomaticProjectionsOnStart)
            {
                serviceCollection.AddHostedService<AutomaticProjectionRebuilderHostedService>();
            }
        }
    }
}