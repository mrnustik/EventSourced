using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourced.Configuration
{
    public class EventSourcedOptions
    {
        public IServiceCollection ServiceCollection { get; }
        public AutomaticProjectionOptions AutomaticProjectionOptions { get; }

        public EventSourcedOptions(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
            AutomaticProjectionOptions = new AutomaticProjectionOptions();
        }
    }

    public class AutomaticProjectionOptions
    {
        public ICollection<Type> RegisteredAutomaticProjections { get; } = new List<Type>();
        public ICollection<Type> RegisteredAutomaticAggregateProjections { get; } = new List<Type>();
    }
}