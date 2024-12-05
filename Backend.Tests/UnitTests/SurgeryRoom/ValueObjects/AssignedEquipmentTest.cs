using System;
using System.Collections.Generic;
using Xunit;
using DDDSample1.Domain.Shared;

public class AssignedEquipmentTests
{
    [Fact]
    public void Constructor_ValidList_ShouldCreateAssignedEquipment()
    {
        // Arrange
        var equipmentList = new List<string> { "Equipment1", "Equipment2" };

        // Act
        var assignedEquipment = new AssignedEquipment(equipmentList);

        // Assert
        Assert.Equal(equipmentList, assignedEquipment.Value);
    }

    [Fact]
    public void Constructor_NullList_ShouldThrowException()
    {
        // Arrange
        List<string> equipmentList = null;

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => new AssignedEquipment(equipmentList));
    }

    [Fact]
    public void Constructor_EmptyList_ShouldThrowException()
    {
        // Arrange
        var equipmentList = new List<string>();

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => new AssignedEquipment(equipmentList));
    }

    [Fact]
    public void ImplicitConversion_ToList_ShouldReturnList()
    {
        // Arrange
        var equipmentList = new List<string> { "Equipment1", "Equipment2" };
        var assignedEquipment = new AssignedEquipment(equipmentList);

        // Act
        List<string> result = assignedEquipment;

        // Assert
        Assert.Equal(equipmentList, result);
    }

    [Fact]
    public void ImplicitConversion_FromList_ShouldReturnAssignedEquipment()
    {
        // Arrange
        var equipmentList = new List<string> { "Equipment1", "Equipment2" };

        // Act
        AssignedEquipment assignedEquipment = equipmentList;

        // Assert
        Assert.Equal(equipmentList, assignedEquipment.Value);
    }

    [Fact]
    public void ToString_ShouldReturnCommaSeparatedString()
    {
        // Arrange
        var equipmentList = new List<string> { "Equipment1", "Equipment2" };
        var assignedEquipment = new AssignedEquipment(equipmentList);
        var expectedString = "Equipment1, Equipment2";

        // Act
        var result = assignedEquipment.ToString();

        // Assert
        Assert.Equal(expectedString, result);
    }
} 