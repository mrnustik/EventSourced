using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;
using EventSourced.Domain.Events;
using EventSourced.Persistence;
using FluentAssertions;
using Moq;
using Xunit;

namespace EventSourced.Tests.Persistence
{
    public class RepositoryTests
    {
        private readonly Mock<IEventStore> eventStoreMock = new();

        [Fact]
        public async Task SaveAsync_WithExistingChanges_DequeuesAllEventsFromAggregate()
        {
            //Arrange
            var testAggregate = new TestAggregate();
            testAggregate.EnqueueTestEvent();
            var repository = CreateSut();

            //Act
            await repository.SaveAsync(testAggregate, CancellationToken.None);

            //Assert
            var domainEventsInQueue = testAggregate.DequeueDomainEvents();
            domainEventsInQueue
                .Should()
                .BeEmpty();
        }

        [Fact]
        public async Task SaveAsync_WithExistingChanges_StoresThemInEventStore()
        {
            //Arrange
            var testAggregate = new TestAggregate();
            testAggregate.EnqueueTestEvent();
            var repository = CreateSut();

            //Act
            await repository.SaveAsync(testAggregate, CancellationToken.None);

            //Assert
            VerifyEventStoreSaveMethodCalled();
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingAggregate_RebuildsItFromEvents()
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
            SetupEventsInEventStore(aggregateId.ToString(), existingEvents);
            var repository = CreateSut();

            //Act
            var aggregate = await repository.GetByIdAsync(aggregateId, CancellationToken.None);

            //Assert
            aggregate.EventsCount
                .Should()
                .Be(4);
        }


        [Fact]
        public async Task GetAllAsync_WithExistingAggregates_RebuildsAllFromEvents()
        {
            //Arrange
            var aggregateId = Guid.NewGuid();
            var aggregateId2 = Guid.NewGuid();
            var existingEvents = new[]
            {
                new TestEvent(),
                new TestEvent(),
                new TestEvent(),
                new TestEvent()
            };
            SetupMultipleAggregateEventsInEventStore(new Dictionary<string, IDomainEvent[]>
            {
                {aggregateId.ToString(), existingEvents.ToArray()},
                {aggregateId2.ToString(), existingEvents.ToArray()}
            });
            var repository = CreateSut();

            //Act
            var aggregates = await repository.GetAllAsync(CancellationToken.None);

            //Assert
            aggregates
                .Should()
                .HaveCount(2);

            aggregates
                .Should()
                .OnlyContain(x => x.EventsCount == 4);
        }

        private void SetupEventsInEventStore(string streamId, IEnumerable<IDomainEvent> domainEvents)
        {
            eventStoreMock
                .Setup(s => s.GetByStreamIdAsync(streamId, It.IsAny<Type>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(domainEvents.ToArray());
        }

        private void SetupMultipleAggregateEventsInEventStore(IDictionary<string, IDomainEvent[]> aggregateToEventsMap)
        {
            eventStoreMock
                .Setup(s => s.GetAllStreamsOfType(It.IsAny<Type>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aggregateToEventsMap);
        }

        private void VerifyEventStoreSaveMethodCalled()
        {
            eventStoreMock.Verify(
                s => s.StoreEventsAsync(It.IsAny<string>(),
                    It.IsAny<Type>(),
                    It.IsAny<IList<IDomainEvent>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        private IRepository<TestAggregate, Guid> CreateSut()
        {
            return new Repository<TestAggregate, Guid>(eventStoreMock.Object);
        }

        private class TestEvent : DomainEvent
        {
        }

        private class TestAggregate : AggregateRoot<Guid>
        {
            public TestAggregate()
                : this(Guid.NewGuid())
            {
            }

            public TestAggregate(Guid id) : base(id)
            {
            }

            public int EventsCount { get; private set; }

            public void EnqueueTestEvent()
            {
                EnqueueAndApplyEvent(new TestEvent());
            }

            public void Apply(TestEvent testEvent)
            {
                EventsCount++;
            }
        }
    }
}