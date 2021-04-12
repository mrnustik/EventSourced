﻿using System;
using EventSourced.Tests.TestDoubles.AggregateRoots;
using EventSourced.Tests.TestDoubles.DomainEvents;
using FluentAssertions;
using Xunit;

namespace EventSourced.Tests.Domain
{
    public class AggregateRootTests
    {
        [Fact]
        public void EnqueuedDomainEvent_IsAddedToQueue_And_CanBeDequeued()
        {
            //Arrange
            var sut = CreateTestAggregateRootWithApplyForTestEvent();
            const string testParameterValue = "Test value";

            //Act
            sut.EnqueueTestDomainEvent(testParameterValue);

            //Assert
            var uncommittedDomainEvents = sut.DequeueDomainEvents();
            uncommittedDomainEvents
                .Should()
                .HaveCount(1)
                .And
                .ContainSingle(e => e.As<TestDomainEvent>().Parameter == testParameterValue);
        }
        
        [Fact]
        public void EnqueuedDomainEvent_AppliesTheEventToAggregate()
        {
            //Arrange
            var sut = CreateTestAggregateRootWithApplyForTestEvent();
            const string testParameterValue = "Test value";

            //Act
            sut.EnqueueTestDomainEvent(testParameterValue);

            //Assert
            sut.ParameterValue
                .Should()
                .Be(testParameterValue);
        }
        
        [Fact]
        public void EnqueuedDomainEvent_DequeuedForSecondTimeShouldBeEmpty()
        {
            //Arrange
            var sut = CreateTestAggregateRootWithApplyForTestEvent();
            const string testParameterValue = "Test value";

            //Act
            sut.EnqueueTestDomainEvent(testParameterValue);
            var firstDequeuedEvents = sut.DequeueDomainEvents();
            var secondDequeuedEvents = sut.DequeueDomainEvents();

            //Assert
            firstDequeuedEvents
                .Should()
                .HaveCount(1);
            secondDequeuedEvents
                .Should()
                .BeEmpty();
        }

        private TestAggregateRootWithApplyForTestEvent CreateTestAggregateRootWithApplyForTestEvent()
        {
            return new TestAggregateRootWithApplyForTestEvent(Guid.NewGuid());
        }
    }
}