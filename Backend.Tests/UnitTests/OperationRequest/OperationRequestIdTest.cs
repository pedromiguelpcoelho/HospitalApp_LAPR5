using System;
using DDDSample1.Domain.OperationRequests;
using Xunit;

namespace DefaultNamespace
{
    public class OperationRequestIdTest
    {
        [Fact]
        public void Constructor_ShouldInitializeWithNewGuid()
        {
            // Act
            var operationRequestId = new OperationRequestId();

            // Assert
            Assert.NotEqual(Guid.Empty, operationRequestId.AsGuid());
        }

        [Fact]
        public void Constructor_ShouldInitializeWithGivenGuid()
        {
            // Arrange
            var guid = Guid.NewGuid();

            // Act
            var operationRequestId = new OperationRequestId(guid);

            // Assert
            Assert.Equal(guid, operationRequestId.AsGuid());
        }

        [Fact]
        public void Constructor_ShouldInitializeWithString()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var guidString = guid.ToString();

            // Act
            var operationRequestId = new OperationRequestId(guidString);

            // Assert
            Assert.Equal(guid, operationRequestId.AsGuid());
        }

        [Fact]
        public void AsString_ShouldReturnGuidAsString()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var operationRequestId = new OperationRequestId(guid);

            // Act
            var result = operationRequestId.AsString();

            // Assert
            Assert.Equal(guid.ToString(), result);
        }
    }
}