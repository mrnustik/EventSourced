using System;
using EventSourced.Domain;
using EventSourced.Domain.Events;
using EventSourced.Helpers;
using EventSourced.Projections;
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

        [Fact]
        public void GetAggregateInformationFromProjection_WithValidProjection_ReturnsAggregateInformation()
        {
            //Act
            var aggregateInformation = ReflectionHelpers.GetAggregateInformationFromProjection(typeof(TestAggregateProjection));

            //Assert
            aggregateInformation
                .aggregateRootType
                .Should()
                .Be(typeof(TestAggregateRoot));
            aggregateInformation
                .aggregateIdType
                .Should()
                .Be(typeof(Guid));
        }

        [Fact]
        public void GetAggregateInformationFromProjection_WithoutValidProjection_Throws()
        {
            //Act
            Action action = () => ReflectionHelpers.GetAggregateInformationFromProjection(typeof(object));
            
            //Assert
            action
                .Should()
                .Throw<ArgumentException>();
        }

        private class TestEvent : DomainEvent
        {
            protected TestEvent(int number)
            {
                Number = number;
            }

            public int Number { get; }
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
        
        private class TestAggregateRoot : AggregateRoot<Guid>
        {
            public TestAggregateRoot(Guid id) : base(id)
            {
            }
        }

        private class TestAggregateProjection : AggregateProjection<TestAggregateRoot, Guid>
        {
            public TestAggregateProjection(Guid id) : base(id)
            {
            }
        }
    }
}