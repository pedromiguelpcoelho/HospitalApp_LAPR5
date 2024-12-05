using System;
using DDDSample1.Domain.OperationRequests;
using Xunit;

namespace DefaultNamespace
{
    public class PriorityTest
    {
        [Fact]
        public void Priority_ShouldContainExpectedValues()
        {
            // Assert
            Assert.Equal(0, (int)Priority.Elective);
            Assert.Equal(1, (int)Priority.Urgent);
            Assert.Equal(2, (int)Priority.Emergency);
        }

        [Fact]
        public void Priority_ShouldBeAssignable()
        {
            // Arrange
            Priority priority = Priority.Urgent;

            // Act
            priority = Priority.Emergency;

            // Assert
            Assert.Equal(Priority.Emergency, priority);
        }

        [Fact]
        public void Priority_ShouldThrowException_WhenInvalidValueIsAssigned()
        {
            // Arrange
            Priority priority = (Priority)999;

            // Act & Assert
            Assert.False(Enum.IsDefined(typeof(Priority), priority));
        }
    }
}