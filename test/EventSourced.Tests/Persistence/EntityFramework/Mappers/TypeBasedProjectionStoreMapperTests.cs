using EventSourced.Persistence.EntityFramework.Helpers;
using EventSourced.Persistence.EntityFramework.Mappers;
using FluentAssertions;
using Xunit;

namespace EventSourced.Tests.Persistence.EntityFramework.Mappers
{
    public class TypeBasedProjectionEntityMapperTests
    {
        [Fact]
        public void MapToEntity_WhenUnmapped_KeepsEverythingSet()
        {
            //Arrange
            var projection = new TestProjection(42);
            var sut = CreateSut();
            
            //Act
            var projectionEntity = sut.MapToEntity(projection);
            var unmappedProjection = (TestProjection) sut.MapToProjection(projectionEntity);

            //Assert
            unmappedProjection.NumberWithPrivateSet.Should()
                              .Be(42);
        }
        
        private ITypeBasedProjectionEntityMapper CreateSut()
        {
            return new TypeBasedProjectionEntityMapper(new TypeSerializer());
        }

        private class TestProjection
        {
            public int NumberWithPrivateSet { get; private set; }
            public TestProjection()
            {
            }
            
            public TestProjection(int number)
            {
                NumberWithPrivateSet = number;
            }
        }
    }
}