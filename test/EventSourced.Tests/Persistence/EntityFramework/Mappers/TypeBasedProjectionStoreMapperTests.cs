using EventSourced.Persistence.EntityFramework.Helpers;
using EventSourced.Persistence.EntityFramework.Mappers;
using FluentAssertions;
using Xunit;

namespace EventSourced.Tests.Persistence.EntityFramework.Mappers
{
    public class TypeBasedProjectionStoreMapperTests
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
            unmappedProjection.Number.Should()
                              .Be(42);

            unmappedProjection.NumberWithPrivateSet.Should()
                              .Be(42);

            unmappedProjection.GetProtectedNumber()
                              .Should()
                              .Be(42);

            unmappedProjection.GetPrivateNumber()
                              .Should()
                              .Be(42);
        }
        
        private ITypeBaseProjectionEntityMapper CreateSut()
        {
            return new TypeBaseProjectionEntityMapper(new TypeSerializer());
        }

        private class TestProjection
        {
            public int Number { get; }
            public int NumberWithPrivateSet { get; private set; }
            protected int ProtectedNumber { get; }
            private int PrivateNumber { get; }

            public TestProjection(int number)
            {
                Number = number;
                NumberWithPrivateSet = number;
                ProtectedNumber = number;
                PrivateNumber = number;
            }

            public int GetProtectedNumber() => ProtectedNumber;
            public int GetPrivateNumber() => PrivateNumber;
        }
    }
}