﻿using System.Linq;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.ResourceManagement;
using EventSourced.Diagnostics.Web.Mappers;
using EventSourced.Diagnostics.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Diagnostics.Web.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventSourcedDiagnostics(this IServiceCollection serviceCollection, EventSourcedDiagnosticsOptions options)
        {
            if (serviceCollection.AlreadyContainsDotvvm())
            {
                serviceCollection.Configure((DotvvmConfiguration config) =>
                {
                   DotvvmStartup.ConfigureDiagnostics(config); 
                });
            }
            else
            {
                serviceCollection.AddDotVVM<DotvvmStartup>();
            }
            serviceCollection.AddTransient<IAggregateInformationService, AggregateInformationService>();
            serviceCollection.AddTransient<IProjectionInformationService, ProjectionInformationService>();
            serviceCollection.AddTransient<IAggregateInstancesListItemModelMapper, AggregateInstancesListItemModelMapper>();
            serviceCollection.AddTransient<IExternalEventInformationService, ExternalEventInformationService>();
            serviceCollection.AddSingleton(options);
            return serviceCollection;
        }

        private static bool AlreadyContainsDotvvm(this IServiceCollection serviceCollection)
        {
            return serviceCollection.Any(s => s.ServiceType.Assembly.FullName?.StartsWith("DotVVM.Framework") == false);
        }
    }
}