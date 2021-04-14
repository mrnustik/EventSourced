using System;
using System.Diagnostics.Tracing;
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
        public static EventSourcedOptions UseEntityFrameworkEventStore(this EventSourcedOptions options)
        {
            options.ServiceCollection.AddTransient<IEventStore, EntityFrameworkEventStore>();
            return options;
        }


        public static EventSourcedOptions UseEntityFrameworkSnapshotStore(this EventSourcedOptions options)
        {
            options.ServiceCollection.AddTransient(typeof(ISnapshotStore<>), typeof(EntityFrameworkSnapshotStore<>));
            return options;
        }
        
        public static EventSourcedOptions AddEntityFrameworkSupport(this EventSourcedOptions options, Action<DbContextOptionsBuilder> dbContextOptionsBuilder)
        {
            options.ServiceCollection.AddDbContext<EventSourcedDbContext>(o =>
            {
                dbContextOptionsBuilder(o);
                o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            options.ServiceCollection.AddHostedService<EventSourcedMigrationHostedService>();
            options.ServiceCollection.AddTransient<IDomainEventEntityMapper, DomainEventEntityMapper>();
            options.ServiceCollection.AddTransient<IEventSerializer, EventSerializer>();
            options.ServiceCollection.AddTransient<ITypeSerializer, TypeSerializer>();
            options.ServiceCollection.AddTransient<IAggregateSnapshotEntityMapper, AggregateSnapshotEntityMapper>();
            return options;
        }

    }
}