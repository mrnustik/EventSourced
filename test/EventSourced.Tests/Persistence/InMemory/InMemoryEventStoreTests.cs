using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain.Events;
using EventSourced.Persistence.InMemory;
using FluentAssertions;
using Xunit;

namespace EventSourced.Tests.Persistence.InMemory
{
    public class InMemoryEventStoreTests
    {
        private static readonly Type AnyAggregateType = typeof(object);

        private static readonly Guid AnyStreamId = Guid.NewGuid();

        private static readonly Guid AnyStreamId2 = Guid.NewGuid();

        [Fact]
        public async Task StoreEventsAsync_WithNonExistingAggregate_PreserversOrder()
        {
            //Arrange
            var sut = CreateSut();
            var testEvents = new[]
            {
                new TestEvent(1),
                new TestEvent(2),
                new TestEvent(3)
            };

            //Act
            await sut.StoreEventsAsync(AnyStreamId,
                AnyAggregateType,
                testEvents.Cast<IDomainEvent>().ToList(),
                CancellationToken.None);

            //Assert
            var events = await sut.GetByStreamIdAsync(AnyStreamId, AnyAggregateType, CancellationToken.None);
            events.Should()
                .HaveCount(3)
                .And
                .AllBeOfType<TestEvent>()
                .And
                .Subject
                .Cast<TestEvent>()
                .Should()
                .BeInAscendingOrder(e => e.Number);
        }

        [Fact]
        public async Task StoreEventsAsync_WithExistingAggregate_PreserversOrder()
        {
            //Arrange
            var sut = CreateSut(new Dictionary<StreamIdentification, List<IDomainEvent>>
            {
                {new StreamIdentification(AnyStreamId, AnyAggregateType), new List<IDomainEvent> {new TestEvent(1)}}
            });
            var testEvents = new[]
            {
                new TestEvent(2),
                new TestEvent(3)
            };

            //Act
            await sut.StoreEventsAsync(AnyStreamId,
                AnyAggregateType,
                testEvents.Cast<IDomainEvent>().ToList(),
                CancellationToken.None);

            //Assert
            var events = await sut.GetByStreamIdAsync(AnyStreamId, AnyAggregateType, CancellationToken.None);
            events
                .Should()
                .HaveCount(3)
                .And
                .AllBeOfType<TestEvent>()
                .And
                .Subject
                .Cast<TestEvent>()
                .Should()
                .BeInAscendingOrder(e => e.Number);
        }

        [Fact]
        public async Task GetByStreamIdAsync_WithExistingAggregate_ReturnsEvents()
        {
            //Arrange
            var sut = CreateSut(new Dictionary<StreamIdentification, List<IDomainEvent>>
            {
                {new StreamIdentification(AnyStreamId, AnyAggregateType), new List<IDomainEvent> {new TestEvent(1)}}
            });

            //Act
            var events = await sut.GetByStreamIdAsync(AnyStreamId, AnyAggregateType, CancellationToken.None);

            //Assert
            events
                .Should()
                .HaveCount(1)
                .And
                .ContainSingle(e => e.As<TestEvent>().Number == 1);
        }

        [Fact]
        public async Task GetByStreamIdAsync_WithNonExistingAggregate_Throws()
        {
            //Arrange
            var sut = CreateSut();

            //Act
            Func<Task> action = () => sut.GetByStreamIdAsync(AnyStreamId, AnyAggregateType, CancellationToken.None);

            //Assert
            await action
                .Should()
                .ThrowAsync<Exception>();
        }

        [Fact]
        public async Task GetAllByStreamType_WithEmptyValue_ReturnsEmpty()
        {
            //Arrange
            var sut = CreateSut();

            //Act
            var dictionary = await sut.GetAllStreamsOfType(AnyAggregateType, CancellationToken.None);

            //Assert
            dictionary
                .Should()
                .BeEmpty();
        }


        [Fact]
        public async Task GetAllByStreamType_WithExistingValues_ReturnsEventsOfAggregateType()
        {
            //Arrange
            var sut = CreateSut(
                new Dictionary<StreamIdentification, List<IDomainEvent>>
                {
                    {
                        new StreamIdentification(AnyStreamId, AnyAggregateType),
                        new List<IDomainEvent> {new TestEvent(1), new TestEvent(1)}
                    },
                    {new StreamIdentification(AnyStreamId2, AnyAggregateType), new List<IDomainEvent> {new TestEvent(1)}}
                });

            //Act
            var dictionary = await sut.GetAllStreamsOfType(AnyAggregateType, CancellationToken.None);

            //Assert
            dictionary
                .Should()
                .HaveCount(2);

            dictionary[AnyStreamId]
                .Should()
                .HaveCount(2);

            dictionary[AnyStreamId2]
                .Should()
                .HaveCount(1);
        }

        [Fact]
        public async Task GetEventsOfTypeAsync_WithExistingValues_ReturnsEventsOfType()
        {
            //Arrange
            var sut = CreateSut(
                new Dictionary<StreamIdentification, List<IDomainEvent>>
                {
                    {
                        new StreamIdentification(AnyStreamId, AnyAggregateType),
                        new List<IDomainEvent> {new TestEvent(1), new TestEvent(1)}
                    },
                    {new StreamIdentification(AnyStreamId2, AnyAggregateType), new List<IDomainEvent> {new TestEvent(1)}}
                });

            //Act
            var dictionary = await sut.GetEventsOfTypeAsync(typeof(TestEvent), CancellationToken.None);

            //Assert
            dictionary
                .Should()
                .HaveCount(3);
        }
        
        [Fact]
        public async Task StreamExistsAsync_WithExistingStream_ReturnsTrue()
        {
            //Arrange
            var sut = CreateSut(
                new Dictionary<StreamIdentification, List<IDomainEvent>>
                {
                    {
                        new StreamIdentification(AnyStreamId, AnyAggregateType),
                        new List<IDomainEvent> {new TestEvent(1), new TestEvent(1)}
                    }
                });

            //Act
            var streamExists = await sut.StreamExistsAsync(AnyStreamId, AnyAggregateType, CancellationToken.None);

            //Assert
            streamExists
                .Should()
                .BeTrue();
        }
        
        [Fact]
        public async Task StreamExistsAsync_WithNonExistingStream_ReturnsFalse()
        {
            //Arrange
            var sut = CreateSut();

            //Act
            var streamExists = await sut.StreamExistsAsync(AnyStreamId, AnyAggregateType, CancellationToken.None);

            //Assert
            streamExists
                .Should()
                .BeFalse();
        }

        private InMemoryEventStore CreateSut()
        {
            return new();
        }

        private InMemoryEventStore CreateSut(Dictionary<StreamIdentification, List<IDomainEvent>> dictionary)
        {
            return new(dictionary);
        }

        private class TestEvent : DomainEvent
        {
            public TestEvent(int number)
            {
                Number = number;
            }

            public int Number { get; }
        }
    }
}