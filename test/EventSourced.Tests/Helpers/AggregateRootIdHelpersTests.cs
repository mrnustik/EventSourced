using System;
using EventSourced.Helpers;
using FluentAssertions;
using Xunit;

namespace EventSourced.Tests.Helpers
{
    public class AggregateRootIdHelpersTests
    {
        [Fact]
        public void ToAggregateRootId_WithGuidType_PerformsConversion()
        {
            //Arrange
            var stringGuid = "7D15190F-2B78-40C2-9BBB-90CC421C1464";
            var guid = new Guid(stringGuid);
            
            //Act
            var aggregateRootId = stringGuid.ToAggregateRootId<Guid>();

            //Assert
            aggregateRootId
                .Should()
                .Be(guid);
        }
        
        [Fact]
        public void ToAggregateRootId_WithIntegerType_PerformsConversion()
        {
            //Arrange
            var stringGuid = "42";
            var id = 42;
            
            //Act
            var aggregateRootId = stringGuid.ToAggregateRootId<int>();

            //Assert
            aggregateRootId
                .Should()
                .Be(id);
        }
        
                
        [Fact]
        public void ToAggregateRootId_WithStringType_PerformsConversion()
        {
            //Arrange
            var stringGuid = "42";
            var id = "42";
            
            //Act
            var aggregateRootId = stringGuid.ToAggregateRootId<string>();

            //Assert
            aggregateRootId
                .Should()
                .Be(id);
        }
    }
}