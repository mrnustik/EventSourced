using System;
using EventSourced.Domain.Events;
using EventSourced.Helpers;
using FluentAssertions;
using Xunit;

namespace EventSourced.Tests.Helpers
{
    public class EventApplicatorTests
    {
        [Theory]
        [InlineData(typeof(ObjectWithPublicApplyEvent))]
        [InlineData(typeof(ObjectWithProtectedApplyEvent))]
        [InlineData(typeof(ObjectWithPrivateApplyEvent))]
        public void ApplyEventsToObject_ApplyMethodIsCalled_InAllInstances(Type type)
        {
            //Arrange
            var testObject = (IObjectWithNumber) Activator.CreateInstance(type);
            var testEvent = new TestEvent(42);

            //Act
            testObject!.ApplyEventsToObject(testEvent);

            //Assert
            testObject.Number.Should()
                      .Be(42);
        }

        [Theory]
        [InlineData(typeof(ObjectWithPublicApplyEvent))]
        [InlineData(typeof(ObjectWithProtectedApplyEvent))]
        [InlineData(typeof(ObjectWithPrivateApplyEvent))]
        public void ApplyEventsToObject_WithMissingApplyMethod_FailsInAllInstances(Type type)
        {
            //Arrange
            var testObject = (IObjectWithNumber) Activator.CreateInstance(type);
            var testEvent = new OtherTestEvent(42);

            //Act
            Action act = () => testObject!.ApplyEventsToObject(testEvent);

            //Assert
            act.Should()
               .Throw<ArgumentException>();
        }

        private class TestEvent : DomainEvent
        {
            public int Number { get; }

            public TestEvent(int number)
            {
                Number = number;
            }
        }

        private class OtherTestEvent : DomainEvent
        {
            public int Number { get; }

            public OtherTestEvent(int number)
            {
                Number = number;
            }
        }

        private interface IObjectWithNumber
        {
            public int Number { get; }
        }

        private class ObjectWithPublicApplyEvent : IObjectWithNumber
        {
            public int Number { get; private set; }

            public void Apply(TestEvent testEvent)
            {
                Number = testEvent.Number;
            }
        }

        private class ObjectWithProtectedApplyEvent : IObjectWithNumber
        {
            public int Number { get; private set; }

            protected void Apply(TestEvent testEvent)
            {
                Number = testEvent.Number;
            }
        }

        private class ObjectWithPrivateApplyEvent : IObjectWithNumber
        {
            public int Number { get; private set; }

            private void Apply(TestEvent testEvent)
            {
                Number = testEvent.Number;
            }
        }
    }
}