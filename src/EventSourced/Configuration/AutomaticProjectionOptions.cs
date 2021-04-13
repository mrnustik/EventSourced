using System;
using System.Collections.Generic;

namespace EventSourced.Configuration
{
    public class AutomaticProjectionOptions
    {
        public ICollection<Type> RegisteredAutomaticProjections { get; } = new List<Type>();
        public ICollection<Type> RegisteredAutomaticAggregateProjections { get; } = new List<Type>();
    }
}