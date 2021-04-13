using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EventSourced.Domain.Events;

namespace EventSourced.Helpers
{
    internal static class ReflectionHelpers
    {
        private const BindingFlags DefaultApplyMethodBindingFlags =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static MethodInfo? GetApplyMethodForEventInObject(object @object, IDomainEvent domainEvent)
        {
            var aggregateType = @object.GetType();
            var methods = aggregateType.GetMethods(DefaultApplyMethodBindingFlags);
            return methods
                .Where(m => m.Name == "Apply")
                .SingleOrDefault(m =>
                    m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == domainEvent.GetType());
        }

        public static IEnumerable<Type> GetTypesOfDomainEventsApplicableToObject(Type objectType)
        {
            return objectType.GetMethods(DefaultApplyMethodBindingFlags)
                .Where(m => m.Name.Equals("Apply"))
                .Where(m => m.GetParameters().Length == 1)
                .SelectMany(m => m.GetParameters())
                .Where(p => p.ParameterType.IsAssignableTo(typeof(IDomainEvent)))
                .Select(p => p.ParameterType);
        }
    }
}