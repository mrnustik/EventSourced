using System;
using EventSourced.Domain;
using BindingFlags = System.Reflection.BindingFlags;

namespace EventSourced.Helpers
{
    public class AggregateRootFactory
    {
        public static TAggregateRoot CreateAggregateRoot<TAggregateRoot>(Guid id) where TAggregateRoot : AggregateRoot =>
            (TAggregateRoot) CreateAggregateRoot(id, typeof(TAggregateRoot));

        public static AggregateRoot CreateAggregateRoot(Guid id, Type type)
        {
            var constructorInfo = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public,
                                                      null,
                                                      new[] {typeof(Guid)},
                                                      null);
            if (constructorInfo == null)
            {
                throw new ArgumentException($"Could not find constructor accepting a Guid on aggregate of type {type.FullName}");
            }
            else
            {
                return (AggregateRoot) constructorInfo.Invoke(new object?[] {id});
            }
        }
    }
}