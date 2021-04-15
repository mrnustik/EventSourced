using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;
using EventSourced.Persistence;
using EventSourced.Persistence.EntityFramework;
using EventSourced.Persistence.EntityFramework.Helpers;
using EventSourced.Persistence.EntityFramework.Mappers;
using FluentAssertions;
using Xunit;

namespace EventSourced.Tests.Persistence.EntityFramework
{
    public class EntityFrameworkSnapshotStoreTests
    {
        [Fact]
        public async Task StoreSnapshotAsync_WithExistingValues_ReloadsItCorrectly()
        {
            //Arrange
            var aggregateId = Guid.NewGuid();
            var testAggregate = new TestAggregate(aggregateId);
            testAggregate.SetTitle("42");
            var sut = CreateSut();

            //Act
            await sut.StoreSnapshotAsync(testAggregate, CancellationToken.None);
            testAggregate.SetTitle("420");
            var loadedSnapshot = await sut.LoadSnapshotAsync(aggregateId, CancellationToken.None);

            //Assert
            loadedSnapshot.Should()
                          .NotBeNull();

            loadedSnapshot!.Title.Should()
                           .Be("42");
        }

        [Fact]
        public async Task StoreSnapshot_WithNonExistingValue_ReturnsNull()
        {
            //Arrange
            var aggregateId = Guid.NewGuid();
            var sut = CreateSut();

            //Act
            var loadedSnapshot = await sut.LoadSnapshotAsync(aggregateId, CancellationToken.None);

            //Assert
            loadedSnapshot.Should()
                          .BeNull();
        }

        private ISnapshotStore<TestAggregate> CreateSut()
        {
            var typeSerializer = new TypeSerializer();
            return new EntityFrameworkSnapshotStore<TestAggregate>(new AggregateSnapshotEntityMapper(typeSerializer),
                                                                   TestDbContextFactory.Create(),
                                                                   new TypeSerializer());
        }

        private class TestAggregate : AggregateRoot
        {
            public string Title { get; private set; }

            public TestAggregate(Guid id)
                : base(id)
            {
            }

            public void SetTitle(string title)
            {
                Title = title;
            }
        }
    }
}