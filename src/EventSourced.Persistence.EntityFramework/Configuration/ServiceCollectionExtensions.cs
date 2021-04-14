using System;
using EventSourced.Configuration;
using EventSourced.Persistence.EntityFramework.Entities;
using EventSourced.Persistence.EntityFramework.Helpers;
using EventSourced.Persistence.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Persistence.EntityFramework.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static EventSourcedOptions UseEntityFrameworkEventStore(this EventSourcedOptions options,
                                                                       Action<DbContextOptionsBuilder> dbContextOptionsBuilder)
        {
            options.ServiceCollection.AddDbContext<EventSourcedDbContext>( o =>
            {
                dbContextOptionsBuilder(o);
                o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            options.ServiceCollection.AddHostedService<EventSourcedMigrationHostedService>();
            options.ServiceCollection.AddTransient<IEventStore, EntityFrameworkEventStore>();
            options.ServiceCollection.AddTransient<IDomainEventEntityMapper, DomainEventEntityMapper>();
            options.ServiceCollection.AddTransient<IEventSerializer, EventSerializer>();
            options.ServiceCollection.AddTransient<ITypeSerializer, TypeSerializer>();
            return options;
        }
    }
}