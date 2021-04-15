﻿using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Domain;
using EventSourced.Persistence;
using EventSourced.Persistence.EntityFramework;
using EventSourced.Persistence.EntityFramework.Helpers;
using EventSourced.Persistence.EntityFramework.Mappers;
using EventSourced.Projections;
using FluentAssertions;
using Xunit;

namespace EventSourced.Tests.Persistence.EntityFramework
{
    public class EntityFrameworkProjectionStoreTests
    {
        [Fact]
        public async Task LoadProjectionAsync_WithPreviouslyStoredProjection_ReturnsIt()
        {
            //Arrange
            var typeBasedProjection = new TestTypeBasedProjection(42);
            var sut = CreateSut(TestDbContextFactory.Create());

            //Act
            await sut.StoreProjectionAsync(typeBasedProjection, CancellationToken.None);
            var loadedProjection = await sut.LoadProjectionAsync<TestTypeBasedProjection>(CancellationToken.None);

            //Assert
            loadedProjection.Should()
                            .NotBeNull();

            loadedProjection!.Value.Should()
                             .Be(42);
        }

        [Fact]
        public async Task LoadProjectionAsync_WithPreviouslyUpdatedProjection_ReturnsIt()
        {
            //Arrange
            var typeBasedProjection = new TestTypeBasedProjection(42);
            var dbContext = TestDbContextFactory.Create();
            var sut = CreateSut(dbContext);

            //Act
            await sut.StoreProjectionAsync(typeBasedProjection, CancellationToken.None);
            typeBasedProjection.SetValue(420);
            dbContext.ChangeTracker.Clear();
            await sut.StoreProjectionAsync(typeBasedProjection, CancellationToken.None);
            var loadedProjection = await sut.LoadProjectionAsync<TestTypeBasedProjection>(CancellationToken.None);

            //Assert
            loadedProjection.Should()
                            .NotBeNull();

            loadedProjection!.Value.Should()
                             .Be(420);
        }

        [Fact]
        public async Task LoadProjectionAsync_WithoutExistingValue_ReturnsNull()
        {
            //Arrange
            var sut = CreateSut(TestDbContextFactory.Create());

            //Act
            var loadedProjection = await sut.LoadProjectionAsync<TestTypeBasedProjection>(CancellationToken.None);

            //Assert
            loadedProjection.Should()
                            .BeNull();
        }

        [Fact]
        public async Task LoadAggregateProjectionAsync_WithPreviouslyStoredProjection_ReturnsIt()
        {
            //Arrange
            var aggregateId = Guid.NewGuid();
            var aggregateProjection = new TestAggregateBasedProjection(aggregateId);
            aggregateProjection.SetValue(42);
            var dbContext = TestDbContextFactory.Create();
            var sut = CreateSut(dbContext);

            //Act
            await sut.StoreAggregateProjectionAsync(aggregateId, aggregateProjection, CancellationToken.None);
            var loadedProjection =
                await sut.LoadAggregateProjectionAsync<TestAggregateBasedProjection, TestAggregateRoot>(
                    aggregateId,
                    CancellationToken.None);

            //Assert
            loadedProjection.Should()
                            .NotBeNull();

            loadedProjection!.Value.Should()
                             .Be(42);
        }

        [Fact]
        public async Task LoadAggregateProjectionAsync_WithoutExistingValue_ReturnsNull()
        {
            //Arrange
            var aggregateId = Guid.NewGuid();
            var sut = CreateSut(TestDbContextFactory.Create());

            //Act
            var loadedProjection =
                await sut.LoadAggregateProjectionAsync<TestAggregateBasedProjection, TestAggregateRoot>(
                    aggregateId,
                    CancellationToken.None);

            //Assert
            loadedProjection.Should()
                            .BeNull();
        }

        [Fact]
        public async Task LoadAggregateProjectionAsync_WithPreviouslyUpdatedProjection_ReturnsIt()
        {
            //Arrange
            var aggregateId = Guid.NewGuid();
            var aggregateProjection = new TestAggregateBasedProjection(aggregateId);
            aggregateProjection.SetValue(42);
            var dbContext = TestDbContextFactory.Create();
            var sut = CreateSut(dbContext);

            //Act
            await sut.StoreAggregateProjectionAsync(aggregateId, aggregateProjection, CancellationToken.None);
            aggregateProjection.SetValue(420);
            dbContext.ChangeTracker.Clear();
            await sut.StoreAggregateProjectionAsync(aggregateId, aggregateProjection, CancellationToken.None);
            var loadedProjection =
                await sut.LoadAggregateProjectionAsync<TestAggregateBasedProjection, TestAggregateRoot>(
                    aggregateId,
                    CancellationToken.None);

            //Assert
            loadedProjection.Should()
                            .NotBeNull();

            loadedProjection!.Value.Should()
                             .Be(420);
        }

        private IProjectionStore CreateSut(EventSourcedDbContext dbContext)
        {
            var typeSerializer = new TypeSerializer();
            return new EntityFrameworkProjectionStore(new TypeBasedProjectionEntityMapper(typeSerializer),
                                                      dbContext,
                                                      typeSerializer,
                                                      new AggregateBasedProjectionEntityMapper(typeSerializer));
        }

        private class TestTypeBasedProjection
        {
            public int Value { get; private set; }

            public TestTypeBasedProjection(int value)
            {
                Value = value;
            }

            public void SetValue(int value)
            {
                Value = value;
            }
        }

        private class TestAggregateBasedProjection : AggregateProjection<TestAggregateRoot>
        {
            public int Value { get; private set; }

            public TestAggregateBasedProjection(Guid id)
                : base(id)
            {
            }

            public void SetValue(int value)
            {
                Value = value;
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