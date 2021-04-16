﻿using EventSourced.Sample.Warehouse.Domain.ImportLocation.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Sample.Warehouse.Domain.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICreateImportLocationDomainService, CreateImportLocationDomainService>();
            return serviceCollection;
        }
    }
}