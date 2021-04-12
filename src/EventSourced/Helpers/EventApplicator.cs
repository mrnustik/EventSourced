using System;
using System.Collections.Generic;
using EventSourced.Abstractions.Domain.Events;

namespace EventSourced.Helpers
{
    internal static class EventApplicator
    {
        public static void ApplyEventsToObject(this object @object, params IDomainEvent[] domainEvents)
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
