using System;
using System.Collections.Generic;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.StaffProfile;
using Xunit;

public class OperationTypeTest
{
    [Fact]
    public void Constructor_ShouldCreateOperationType_WhenValidParameters()
    {
        // Arrange
        var name = new Name("Appendectomy");
        var requiredStaff = new List<StaffId> { new StaffId(Guid.NewGuid()) };
        var estimatedDuration = 60;

        // Act
        var operationType = new OperationType(name, requiredStaff, estimatedDuration);

        // Assert
        Assert.Equal(name, operationType.Name);
        Assert.Equal(requiredStaff, operationType.RequiredStaff);
        Assert.Equal(estimatedDuration, operationType.EstimatedDuration);
        Assert.True(operationType.isActive);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenNameIsNull()
    {
        // Arrange
        List<StaffId> requiredStaff = new List<StaffId> { new StaffId(Guid.NewGuid()) };
        int estimatedDuration = 60;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new OperationType(null, requiredStaff, estimatedDuration));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenRequiredStaffIsNull()
    {
        // Arrange
        var name = new Name("Appendectomy");
        int estimatedDuration = 60;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new OperationType(name, null, estimatedDuration));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenEstimatedDurationIsZeroOrNegative()
    {
        // Arrange
        var name = new Name("Appendectomy");
        var requiredStaff = new List<StaffId> { new StaffId(Guid.NewGuid()) };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new OperationType(name, requiredStaff, 0));
        Assert.Throws<ArgumentException>(() => new OperationType(name, requiredStaff, -1));
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateOperationType_WhenValidParameters()
    {
        // Arrange
        var name = new Name("Appendectomy");
        var requiredStaff = new List<StaffId> { new StaffId(Guid.NewGuid()) };
        var estimatedDuration = 60;
        var operationType = new OperationType(name, requiredStaff, estimatedDuration);

        var newName = new Name("Cholecystectomy");
        var newRequiredStaff = new List<StaffId> { new StaffId(Guid.NewGuid()) };
        var newEstimatedDuration = 90;

        // Act
        operationType.UpdateDetails(newName, newRequiredStaff, newEstimatedDuration);

        // Assert
        Assert.Equal(newName, operationType.Name);
        Assert.Equal(newRequiredStaff, operationType.RequiredStaff);
        Assert.Equal(newEstimatedDuration, operationType.EstimatedDuration);
    }

    [Fact]
    public void UpdateDetails_ShouldThrowArgumentNullException_WhenNameIsNull()
    {
        // Arrange
        var name = new Name("Appendectomy");
        var requiredStaff = new List<StaffId> { new StaffId(Guid.NewGuid()) };
        var estimatedDuration = 60;
        var operationType = new OperationType(name, requiredStaff, estimatedDuration);

        var newRequiredStaff = new List<StaffId> { new StaffId(Guid.NewGuid()) };
        var newEstimatedDuration = 90;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => operationType.UpdateDetails(null, newRequiredStaff, newEstimatedDuration));
    }

    [Fact]
    public void UpdateDetails_ShouldThrowArgumentNullException_WhenRequiredStaffIsNull()
    {
        // Arrange
        var name = new Name("Appendectomy");
        var requiredStaff = new List<StaffId> { new StaffId(Guid.NewGuid()) };
        var estimatedDuration = 60;
        var operationType = new OperationType(name, requiredStaff, estimatedDuration);

        var newName = new Name("Cholecystectomy");
        var newEstimatedDuration = 90;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => operationType.UpdateDetails(newName, null, newEstimatedDuration));
    }

    [Fact]
    public void UpdateDetails_ShouldThrowArgumentException_WhenEstimatedDurationIsZeroOrNegative()
    {
        // Arrange
        var name = new Name("Appendectomy");
        var requiredStaff = new List<StaffId> { new StaffId(Guid.NewGuid()) };
        var estimatedDuration = 60;
        var operationType = new OperationType(name, requiredStaff, estimatedDuration);

        var newName = new Name("Cholecystectomy");
        var newRequiredStaff = new List<StaffId> { new StaffId(Guid.NewGuid()) };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => operationType.UpdateDetails(newName, newRequiredStaff, 0));
        Assert.Throws<ArgumentException>(() => operationType.UpdateDetails(newName, newRequiredStaff, -1));
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var name = new Name("Appendectomy");
        var requiredStaff = new List<StaffId> { new StaffId(Guid.NewGuid()) };
        var estimatedDuration = 60;
        var operationType = new OperationType(name, requiredStaff, estimatedDuration);

        // Act
        operationType.Deactivate();

        // Assert
        Assert.False(operationType.isActive);
    }
}