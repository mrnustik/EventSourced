﻿using EventSourced.Domain;
using EventSourced.Projections;

namespace EventSourced.Configuration
{
    public static class EventSourcedOptionsExtensions
    {
        public static void RegisterAutomaticProjection<TProjection>(this EventSourcedOptions options)
            where TProjection : new()
        {
            options.AutomaticProjectionOptions.RebuildAutomaticProjectionsOnStart = true;
            options.AutomaticProjectionOptions.RegisteredAutomaticProjections.Add(typeof(TProjection));
        }
        
        public static void RegisterAutomaticAggregateProjection<TProjection, TAggregateRoot>(this EventSourcedOptions options)
            where TProjection : AggregateProjection<TAggregateRoot>
            where TAggregateRoot : AggregateRoot
        {
            options.AutomaticProjectionOptions.RebuildAutomaticProjectionsOnStart = true;
            options.AutomaticProjectionOptions.RegisteredAutomaticAggregateProjections.Add(typeof(TProjection));
        }
    }
}