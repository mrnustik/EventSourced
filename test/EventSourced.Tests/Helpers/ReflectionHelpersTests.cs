using System;
using EventSourced.Domain.Events;
using EventSourced.Helpers;
using FluentAssertions;
using Xunit;

namespace EventSourced.Tests.Helpers
{
    public class ReflectionHelpersTests
    {

        [Theory]
        [InlineData(typeof(ObjectWithPublicApplyEvent))]
        [InlineData(typeof(ObjectWithProtectedApplyEvent))]
        [InlineData(typeof(ObjectWithPrivateApplyEvent))]
        public void ApplyEventsToObject_ApplyMethodIsCalled_InAllInstances(Type type)
        {
            //Act
            var applicableEvents = ReflectionHelpers.GetTypesOfDomainEventsApplicableToObject(type);

            //Assert
            applicableEvents
                .Should()
                .HaveCount(1)
                .And
                .ContainSingle(t => t == typeof(TestEvent));
        }

        private class TestEvent : DomainEvent
        {
            public int Number { get; }

            protected TestEvent(int number)
            {
                Number = number;
            }
        }
        
        private class ObjectWithPublicApplyEvent
        {
            public int Number { get; private set; }
            
            public void Apply(TestEvent testEvent)
            {
                Number = testEvent.Number;
            }
        }
        
        private class ObjectWithProtectedApplyEvent 
        {
            public int Number { get; private set; }
            
            protected void Apply(TestEvent testEvent)
            {
                Number = testEvent.Number;
            }
        }
        
        private class ObjectWithPrivateApplyEvent 
        {
            public int Number { get; private set; }
            
            private void Apply(TestEvent testEvent)
            {
                Number = testEvent.Number;
            }
        }
    }
}