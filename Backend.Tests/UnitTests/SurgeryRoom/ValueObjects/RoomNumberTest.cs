using System;
using Xunit;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.SurgeryRooms;

public class RoomNumberTests
{
    [Fact]
    public void Constructor_ValidValue_ShouldCreateRoomNumber()
    {
        // Arrange
        int value = 10;

        // Act
        var roomNumber = new RoomNumber(value);

        // Assert
        Assert.Equal(value, roomNumber.Value);
    }

    [Fact]
    public void Constructor_ZeroValue_ShouldThrowException()
    {
        // Arrange
        int value = 0;

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => new RoomNumber(value));
    }

    [Fact]
    public void Constructor_NegativeValue_ShouldThrowException()
    {
        // Arrange
        int value = -5;

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => new RoomNumber(value));
    }

    [Fact]
    public void ImplicitConversion_ToInt_ShouldReturnInt()
    {
        // Arrange
        int value = 10;
        var roomNumber = new RoomNumber(value);

        // Act
        int result = roomNumber;

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void ImplicitConversion_FromInt_ShouldReturnRoomNumber()
    {
        // Arrange
        int value = 10;

        // Act
        RoomNumber roomNumber = value;

        // Assert
        Assert.Equal(value, roomNumber.Value);
    }

    [Fact]
    public void ToString_ShouldReturnStringValue()
    {
        // Arrange
        int value = 10;
        var roomNumber = new RoomNumber(value);
        var expectedString = "10";

        // Act
        var result = roomNumber.ToString();

        // Assert
        Assert.Equal(expectedString, result);
    }
}