using System;
using Xunit;
using DDDSample1.Domain.Shared;

public class MaintenanceSlotTests
{
    [Fact]
    public void Constructor_ValidTimes_ShouldCreateMaintenanceSlot()
    {
        // Arrange
        DateTime startTime = new DateTime(2023, 10, 1, 8, 0, 0);
        DateTime endTime = new DateTime(2023, 10, 1, 10, 0, 0);

        // Act
        var slot = new MaintenanceSlot(startTime, endTime);

        // Assert
        Assert.Equal(startTime, slot.StartTime);
        Assert.Equal(endTime, slot.EndTime);
    }

    [Fact]
    public void Constructor_EndTimeBeforeStartTime_ShouldThrowException()
    {
        // Arrange
        DateTime startTime = new DateTime(2023, 10, 1, 10, 0, 0);
        DateTime endTime = new DateTime(2023, 10, 1, 8, 0, 0);

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => new MaintenanceSlot(startTime, endTime));
    }

    [Fact]
    public void Constructor_EndTimeEqualToStartTime_ShouldThrowException()
    {
        // Arrange
        DateTime startTime = new DateTime(2023, 10, 1, 8, 0, 0);
        DateTime endTime = new DateTime(2023, 10, 1, 8, 0, 0);

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => new MaintenanceSlot(startTime, endTime));
    }

        [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        DateTime startTime = new DateTime(2023, 10, 1, 8, 0, 0);
        DateTime endTime = new DateTime(2023, 10, 1, 10, 0, 0);
        var slot = new MaintenanceSlot(startTime, endTime);
        var expectedString = "01/10/2023 08:00:00 - 01/10/2023 10:00:00";
    
        // Act
        var result = slot.ToString();
    
        // Assert
        Assert.Equal(expectedString, result);
    }
}