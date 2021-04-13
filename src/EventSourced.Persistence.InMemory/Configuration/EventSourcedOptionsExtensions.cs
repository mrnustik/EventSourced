﻿using EventSourced.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Persistence.InMemory.Configuration
{
    public static class EventSourcedOptionsExtensions
    {
        public static void UseInMemoryEventStore(this EventSourcedOptions options)
        {
            options.ServiceCollection.AddSingleton<IEventStore, InMemoryEventStore>();
        }
    }
}