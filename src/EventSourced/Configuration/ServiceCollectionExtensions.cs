using System;
using EventSourced.Domain.Events;
using EventSourced.EventBus;
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
        public static IServiceCollection AddEventSourced(this IServiceCollection serviceCollection, Action<EventSourcedOptions> optionsConfiguration)
        {
            serviceCollection.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            serviceCollection.AddTransient<IManualProjectionBuilder, ManualProjectionBuilder>();
            serviceCollection.AddTransient<IEventStreamUpdatedEventHandler, AutomaticProjectionEventStreamUpdatedEventHandler>();
            serviceCollection.AddTransient<IEventStreamUpdatedEventHandler, AutomaticAggregateProjectionEventStreamUpdatedEventHandler>();
            serviceCollection.AddTransient<IAutomaticProjectionRebuilder, AutomaticProjectionRebuilder>();
            serviceCollection.AddTransient(typeof(ISnapshotStore<>), typeof(NullSnapshotStore<>));
            serviceCollection.AddTransient<ISnapshotCreationStrategy, NullSnapshotCreationStrategy>();
            serviceCollection.AddTransient<IDomainEventBus, InProcessDomainEventBus>();
            
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
            return serviceCollection;
        }

        public static IServiceCollection RegisterDomainEventHandler<TDomainEventHandler, TDomainEvent>(
            this IServiceCollection serviceCollection) 
            where TDomainEventHandler : class, IDomainEventHandler<TDomainEvent>
            where TDomainEvent : DomainEvent
        {
            serviceCollection.AddTransient<IDomainEventHandler<TDomainEvent>, TDomainEventHandler>();
            return serviceCollection;
        }
    }
}