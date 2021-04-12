﻿using System.Linq;
using System.Reflection;
using EventSourced.Abstractions.Domain.Events;
using EventSourced.Domain;
using EventSourced.Domain.Events;

namespace EventSourced.Helpers
{
    public static class ReflectionHelpers
    {
        public static MethodInfo? GetApplyMethodForEventInObject(object @object, IDomainEvent domainEvent)
        {
            var aggregateType = @object.GetType();
            var methods = aggregateType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return methods
                .Where(m => m.Name == "Apply")
                .SingleOrDefault(m =>
                    m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == domainEvent.GetType());
        }
    }
}