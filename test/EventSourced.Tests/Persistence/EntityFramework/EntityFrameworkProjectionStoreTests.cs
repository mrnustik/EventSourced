using System;
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
            var sut = CreateSut();
            
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
        public async Task LoadAggregateProjectionAsync_WithPreviouslyStoredProjection_ReturnsIt()
        {
            //Arrange
            var aggregateId = Guid.NewGuid();
            var aggregateProjection = new TestAggregateBasedProjection(aggregateId);
            aggregateProjection.SetValue(42);
            var sut = CreateSut();
            
            //Act
            await sut.StoreAggregateProjectionAsync(aggregateId, aggregateProjection, CancellationToken.None);
            var loadedProjection = await sut.LoadAggregateProjectionAsync<TestAggregateBasedProjection, TestAggregateRoot>(aggregateId, CancellationToken.None);

            //Assert
            loadedProjection.Should()
                            .NotBeNull();
            
            loadedProjection!.Value.Should()
                             .Be(42);
        }
        
        private IProjectionStore CreateSut()
        {
            var typeSerializer = new TypeSerializer();
            return new EntityFrameworkProjectionStore(new TypeBasedProjectionEntityMapper(typeSerializer),
                                                      TestDbContextFactory.Create(),
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