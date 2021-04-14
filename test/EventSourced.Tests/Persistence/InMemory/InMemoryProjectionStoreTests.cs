using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;
using EventSourced.Persistence;
using EventSourced.Persistence.InMemory;
using EventSourced.Projections;
using FluentAssertions;
using Xunit;

namespace EventSourced.Tests.Persistence.InMemory
{
    public class InMemoryProjectionStoreTests
    {
        [Fact]
        public async Task StoreProjectionAsync_WithValidValue_CanBeThenLoaded()
        {
            //Arrange
            var projection = new TestProjection(42);
            var sut = CreateSut();

            //Act
            await sut.StoreProjectionAsync(projection, CancellationToken.None);

            //Assert
            var loadedProjection = await sut.LoadProjectionAsync<TestProjection>(CancellationToken.None);
            loadedProjection!.Number.Should()
                             .Be(42);
        }

        [Fact]
        public async Task LoadProjectionAsync_WithNonExistingType_ReturnsNull()
        {
            //Arrange
            var sut = CreateSut();

            //Act
            var loadedProjection = await sut.LoadProjectionAsync<TestProjection>(CancellationToken.None);

            //Assert
            loadedProjection.Should()
                            .BeNull();
        }

        [Fact]
        public async Task LoadProjectionAsync_WithExistingType_ReturnsIt()
        {
            //Arrange
            var projection = new TestProjection(42);
            var sut = CreateSut(new Dictionary<Type, object>
            {
                {typeof(TestProjection), projection}
            });

            //Act
            var loadedProjection = await sut.LoadProjectionAsync<TestProjection>(CancellationToken.None);

            //Assert
            loadedProjection.Number.Should()
                            .Be(42);
        }

        [Fact]
        public async Task LoadAggregateProjectionAsync_WithNonExistingType_ReturnsNull()
        {
            //Arrange
            var aggregateRootId = Guid.NewGuid();
            var sut = CreateSut();

            //Act
            var loadedProjection =
                await sut.LoadAggregateProjectionAsync<TestAggregateProjection, TestAggregateRoot>(
                    aggregateRootId,
                    CancellationToken.None);

            //Assert
            loadedProjection.Should()
                            .BeNull();
        }

        [Fact]
        public async Task LoadAggregateProjectionAsync_WithExistingType_ReturnsIt()
        {
            //Arrange
            var aggregateRootId = Guid.NewGuid();
            var sut = CreateSut();
            var storedProjection = new TestAggregateProjection(aggregateRootId);
            await sut.StoreAggregateProjectionAsync(aggregateRootId, storedProjection, CancellationToken.None);

            //Act
            var loadedProjection =
                await sut.LoadAggregateProjectionAsync<TestAggregateProjection, TestAggregateRoot>(
                    aggregateRootId,
                    CancellationToken.None);

            //Assert
            loadedProjection.Should()
                            .NotBeNull();
        }

        private IProjectionStore CreateSut()
        {
            return new InMemoryProjectionStore();
        }

        private IProjectionStore CreateSut(Dictionary<Type, object> originalState)
        {
            return new InMemoryProjectionStore(new ConcurrentDictionary<Type, object>(originalState));
        }

        private class TestProjection
        {
            public int Number { get; }

            public TestProjection(int number)
            {
                Number = number;
            }
        }

        private class TestAggregateProjection : AggregateProjection<TestAggregateRoot>
        {
            public TestAggregateProjection(Guid id)
                : base(id)
            {
            }
        }

        private class TestAggregateRoot : AggregateRoot
        {
            public TestAggregateRoot(Guid id)
                : base(id)
            {
            }
        }
    }
}