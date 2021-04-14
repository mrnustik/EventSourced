using EventSourced.Persistence.EntityFramework.Helpers;
using FluentAssertions;
using Xunit;

namespace EventSourced.Tests.Persistence.EntityFramework.Helpers
{
    public class TypeSerializerTests
    {
        [Fact]
        public void SerializeType_WithExistingType_CanBeDeserialized()
        {
            //Arrange
            var typeToBeSerialized = typeof(TypeSerializerTests);
            var sut = CreateSut();

            //Act
            var serializedType = sut.SerializeType(typeToBeSerialized);
            var deserializedType = sut.DeserializeType(serializedType);

            //Assert
            deserializedType.Should()
                            .Be(typeToBeSerialized);
        }

        private ITypeSerializer CreateSut()
        {
            return new TypeSerializer();
        }
    }
}