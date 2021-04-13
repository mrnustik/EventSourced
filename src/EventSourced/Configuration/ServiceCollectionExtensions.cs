﻿using System;
using EventSourced.Domain.Events;
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
            serviceCollection.AddTransient<IDomainEventHandler, AutomaticProjectionDomainEventHandler>();

            var options = new EventSourcedOptions(serviceCollection);
            optionsConfiguration(options);

            var automaticProjectionsEventMapper = new AutomaticProjectionsEventMapper(options.AutomaticProjectionOptions);
            automaticProjectionsEventMapper.Initialize();
            serviceCollection.AddSingleton<IAutomaticProjectionsEventMapper>(automaticProjectionsEventMapper);
        }
    }
}