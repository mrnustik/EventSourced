﻿namespace EventSourced.Configuration
{
    public static class EventSourcedOptionsExtensions
    {
        public static void RegisterAutomaticProjection<TProjection>(this EventSourcedOptions options)
            where TProjection : new()
        {
            options.AutomaticProjectionOptions.RegisteredAutomaticProjections.Add(typeof(TProjection));
        }
    }
}