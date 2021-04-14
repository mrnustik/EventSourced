using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;
using EventSourced.Persistence;
using EventSourced.Persistence.InMemory;
using FluentAssertions;
using Xunit;

namespace EventSourced.Tests.Persistence.InMemory
{
    public class InMemorySnapshotStore
    {
        [Fact]
        public async Task LoadSnapshotAsync_AfterStoringItFirst_ReturnsTheCopy()
        {
            //Arrange
            var aggregateRoot = new TestAggregateRoot(Guid.NewGuid(), 42);
            var sut = CreateSut();

            //Act
            await sut.StoreSnapshotAsync(aggregateRoot, CancellationToken.None);
            var loadedSnapshot = await sut.LoadSnapshotAsync(aggregateRoot.Id, CancellationToken.None);

            //Assert
            loadedSnapshot.Version
                          .Should()
                          .Be(42);
        }

        private ISnapshotStore<TestAggregateRoot> CreateSut()
        {
            return new InMemorySnapshotStore<TestAggregateRoot>();
        }

        private class TestAggregateRoot : AggregateRoot
        {
            public TestAggregateRoot(Guid id, int version)
                : base(id)
            {
                Version = version;
            }
        }
    }
}