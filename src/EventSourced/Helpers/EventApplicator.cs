﻿using System;
using System.Runtime.CompilerServices;
using EventSourced.Domain.Events;
[assembly: InternalsVisibleTo("EventSourced.Tests")]

namespace EventSourced.Helpers
{
    internal static class EventApplicator
    {
        public static void ApplyEventsToObject(this object @object, params DomainEvent[] domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                var applyMethod = ReflectionHelpers.GetApplyMethodForEventInObject(@object, domainEvent);
                if (applyMethod != null)
                {
                    applyMethod.Invoke(@object, new[] {domainEvent});
                }
                else
                {
                    throw new ArgumentException(
                        $"Missing Apply event for domain event of type {domainEvent.GetType()} on object {@object.GetType()}");
                }
            }
        }
    }
}