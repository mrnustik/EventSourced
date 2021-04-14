using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourced.Configuration;
using EventSourced.Persistence;
using EventSourced.Projections;
using EventSourced.Projections.Automatic;
using Moq;
using Xunit;

namespace EventSourced.Tests.Projections.Automatic
{
    public class AutomaticProjectionRebuilderTests
    {
        private readonly Mock<IManualProjectionBuilder> _manualProjectionBuilderMock = new();
        private readonly Mock<IProjectionStore> _projectionStoreMock = new();
        private readonly Mock<IEventStore> _eventStoreMock = new();

        [Fact]
        public async Task RebuildAllProjectionsAsync_WithRegisteredProjection_RebuildsThem()
        {
            //Arrange
            var options = new AutomaticProjectionOptions();
            options.RegisteredAutomaticProjections.Add(typeof(TestProjection));
            var sut = CreateSut(options);
            _manualProjectionBuilderMock.Setup(s => s.BuildProjectionAsync(It.IsAny<Type>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TestProjection {Number = 42});
            
            //Act
            await sut.RebuildAllRegisteredAutomaticProjections(CancellationToken.None);

            //Assert
            _projectionStoreMock.Verify(s => s.StoreProjectionAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        
        private AutomaticProjectionRebuilder CreateSut(AutomaticProjectionOptions options)
        {
            return new AutomaticProjectionRebuilder(
                options,
                _manualProjectionBuilderMock.Object,
                _projectionStoreMock.Object,
                _eventStoreMock.Object);
        }

        private class TestProjection
        {
            public int Number { get; set; }
        }
    }
}