using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;
using EventSourced.Domain.Events;
using EventSourced.Persistence;
using EventSourced.Projections;
using FluentAssertions;
using Moq;
using Xunit;

namespace EventSourced.Tests.Projections
{
    public class ManualProjectionBuilderTests
    {
        private readonly Mock<IEventStore> _eventStoreMock = new();

        [Fact]
        public async Task BuildProjectionAsync_WithExistingApplicableEvents_BuildsTheProjection()
        {
            //Arrange
            IDomainEvent[] existingEvents =
            {
                new TestEvent(),
                new TestEvent(),
                new TestEvent()
            };
            SetupExistingEventsInEventStore(existingEvents);
            var sut = CreateSut();

            //Act
            var projection = await sut.BuildProjectionAsync<EventCountProjection>(CancellationToken.None);


            //Assert
            projection.AppliedEventsCount
                .Should()
                .Be(3);
        }

        [Fact]
        public async Task BuildAggregateProjectionAsync_WithExistingApplicableEvents_BuildsTheProjection()
        {
            //Arrange
            var aggregateId = Guid.NewGuid();
            var existingEvents = new[]
            {
                new TestEvent(),
                new TestEvent(),
                new TestEvent(),
                new TestEvent()
            };
            SetupEventsInEventStore(aggregateId, existingEvents);
            var sut = CreateSut();

            //Act
            var projection =
                await sut.BuildAggregateProjection<EventCountAggregateProjection, TestAggregateRoot>(aggregateId, CancellationToken.None);

            //Assert
            projection.Id
                .Should()
                .Be(aggregateId);
            
            projection.AppliedEventsCount
                .Should()
                .Be(4);
        }

        private void SetupEventsInEventStore(Guid streamId, IEnumerable<IDomainEvent> domainEvents)
        {
            _eventStoreMock
                .Setup(s => s.GetByStreamIdAsync(streamId, It.IsAny<Type>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(domainEvents.ToArray());
        }

        private void SetupExistingEventsInEventStore(IDomainEvent[] existingEvents)
        {
            _eventStoreMock.Setup(s => s.GetEventsOfTypeAsync(It.IsAny<Type>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvents);
        }

        private IManualProjectionBuilder CreateSut()
        {
            return new ManualProjectionBuilder(_eventStoreMock.Object);
        }

        private class TestAggregateRoot : AggregateRoot
        {
            public TestAggregateRoot(Guid id) : base(id)
            {
            }
        }

        private class TestEvent : DomainEvent
        {
        }

        private class EventCountProjection
        {
            public int AppliedEventsCount { get; private set; }
            
            private void Apply(TestEvent @event)
            {
                AppliedEventsCount++;
            }
        }
        
        private class EventCountAggregateProjection : AggregateProjection<TestAggregateRoot>
        {
            public int AppliedEventsCount { get; private set; }
            
            public EventCountAggregateProjection(Guid id) : base(id)
            {
            }
            
            private void Apply(TestEvent @event)
            {
                AppliedEventsCount++;
            }
        }
    }
}