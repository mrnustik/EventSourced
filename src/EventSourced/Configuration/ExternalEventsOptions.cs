using System;
using System.Collections.Generic;

namespace EventSourced.Configuration
{
    public class ExternalEventsOptions
    {
        public ICollection<Type> RegisteredExternalEvents { get; } = new List<Type>();
    }
}