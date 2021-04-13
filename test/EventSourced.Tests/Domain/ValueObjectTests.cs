using EventSourced.Domain;
using FluentAssertions;
using Xunit;

namespace EventSourced.Tests.Domain
{
    public class ValueObjectTests
    {
        [Fact]
        public void Equals_OnSameObjects_ReturnsTrue()
        {
            //Arrange
            var left = new TestingValueObject(42, "42", true, 42.0M);
            var right = new TestingValueObject(42, "42", true, 42.0M);

            //Act & Assert
            left.Should().Be(right);
            right.Should().Be(left);
        }

        [Fact]
        public void EqualityOperator_OnSameObjects_ReturnsTrue()
        {
            //Arrange
            var left = new TestingValueObject(42, "42", true, 42.0M);
            var right = new TestingValueObject(42, "42", true, 42.0M);

            //Act
            var leftIsRight = left == right;
            var rightIsLeft = right == left;

            //Assert
            leftIsRight.Should().BeTrue();
            rightIsLeft.Should().BeTrue();
        }

        [Fact]
        public void Equals_OnDifferentObjects_ReturnsFalse()
        {
            //Arrange
            var left = new TestingValueObject(42, "42", true, 42.0M);
            var right = new TestingValueObject(42, "420", true, 420.0M);

            //Act & Assert
            left.Should().NotBe(right);
            right.Should().NotBe(left);
        }

        [Fact]
        public void EqualityOperator_OnDifferentObjects_ReturnsFalse()
        {
            //Arrange
            var left = new TestingValueObject(42, "42", true, 42.0M);
            var right = new TestingValueObject(42, "420", true, 420.0M);

            //Act
            var leftIsRight = left == right;
            var rightIsLeft = right == left;

            //Assert
            leftIsRight.Should().BeFalse();
            rightIsLeft.Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_OnSameObjects_ReturnsSame()
        {
            //Arrange
            var left = new TestingValueObject(42, "42", true, 42.0M);
            var right = new TestingValueObject(42, "42", true, 42.0M);

            //Act
            var leftHashCode = left.GetHashCode();
            var rightHashCode = right.GetHashCode();

            //Assert
            leftHashCode.Should().Be(rightHashCode);
        }

        [Fact]
        public void GetHashCode_OnDifferentObjects_ReturnsDifferent()
        {
            //Arrange
            var left = new TestingValueObject(42, "42", true, 42.0M);
            var right = new TestingValueObject(42, "420", true, 420.0M);

            //Act
            var leftHashCode = left.GetHashCode();
            var rightHashCode = right.GetHashCode();

            //Assert
            leftHashCode.Should().NotBe(rightHashCode);
        }

        private class TestingValueObject : ValueObject
        {
            public TestingValueObject(int integerValue, string stringValue, bool booleanValue, decimal decimalValue)
            {
                IntegerValue = integerValue;
                StringValue = stringValue;
                BooleanValue = booleanValue;
                DecimalValue = decimalValue;
            }

            public int IntegerValue { get; }
            public string StringValue { get; }
            public bool BooleanValue { get; }
            public decimal DecimalValue { get; }
        }
    }
}