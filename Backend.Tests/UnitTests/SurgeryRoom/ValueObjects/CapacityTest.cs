using System;
using Xunit;
using DDDSample1.Domain.Shared;

public class CapacityTests
{
    [Fact]
    public void Constructor_ValidValue_ShouldCreateCapacity()
    {
        // Arrange
        int value = 10;

        // Act
        var capacity = new Capacity(value);

        // Assert
        Assert.Equal(value, capacity.Value);
    }

    [Fact]
    public void Constructor_ZeroValue_ShouldThrowException()
    {
        // Arrange
        int value = 0;

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => new Capacity(value));
    }

    [Fact]
    public void Constructor_NegativeValue_ShouldThrowException()
    {
        // Arrange
        int value = -5;

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => new Capacity(value));
    }

    [Fact]
    public void ImplicitConversion_ToInt_ShouldReturnInt()
    {
        // Arrange
        int value = 10;
        var capacity = new Capacity(value);

        // Act
        int result = capacity;

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void ImplicitConversion_FromInt_ShouldReturnCapacity()
    {
        // Arrange
        int value = 10;

        // Act
        Capacity capacity = value;

        // Assert
        Assert.Equal(value, capacity.Value);
    }

    [Fact]
    public void ToString_ShouldReturnStringValue()
    {
        // Arrange
        int value = 10;
        var capacity = new Capacity(value);
        var expectedString = "10";

        // Act
        var result = capacity.ToString();

        // Assert
        Assert.Equal(expectedString, result);
    }
}