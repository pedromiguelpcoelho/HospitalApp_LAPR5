using System;
using Xunit;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.SurgeryRooms;

public class StatusTests
{
    [Fact]
    public void Constructor_ValidValue_ShouldCreateStatus()
    {
        // Arrange
        string value = "Available";

        // Act
        var status = new Status(value);

        // Assert
        Assert.Equal(value, status.Value);
    }

    [Fact]
    public void Constructor_InvalidValue_ShouldThrowException()
    {
        // Arrange
        string value = "InvalidStatus";

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => new Status(value));
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldReturnString()
    {
        // Arrange
        string value = "Occupied";
        var status = new Status(value);

        // Act
        string result = status;

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void ImplicitConversion_FromString_ShouldReturnStatus()
    {
        // Arrange
        string value = "Under maintenance";

        // Act
        Status status = value;

        // Assert
        Assert.Equal(value, status.Value);
    }

    [Fact]
    public void ToString_ShouldReturnStringValue()
    {
        // Arrange
        string value = "Available";
        var status = new Status(value);
        var expectedString = "Available";

        // Act
        var result = status.ToString();

        // Assert
        Assert.Equal(expectedString, result);
    }
}