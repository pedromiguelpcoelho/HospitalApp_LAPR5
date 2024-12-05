using System;
using Xunit;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.SurgeryRooms;

public class RoomTypeTests
{
    [Fact]
    public void Constructor_ValidValue_ShouldCreateRoomType()
    {
        // Arrange
        string value = "Operating Room";

        // Act
        var roomType = new RoomType(value);

        // Assert
        Assert.Equal(value, roomType.Value);
    }

    [Fact]
    public void Constructor_NullValue_ShouldThrowException()
    {
        // Arrange
        string value = null;

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => new RoomType(value));
    }

    [Fact]
    public void Constructor_EmptyValue_ShouldThrowException()
    {
        // Arrange
        string value = "";

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => new RoomType(value));
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldReturnString()
    {
        // Arrange
        string value = "Operating Room";
        var roomType = new RoomType(value);

        // Act
        string result = roomType;

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void ImplicitConversion_FromString_ShouldReturnRoomType()
    {
        // Arrange
        string value = "Operating Room";

        // Act
        RoomType roomType = value;

        // Assert
        Assert.Equal(value, roomType.Value);
    }

    [Fact]
    public void ToString_ShouldReturnStringValue()
    {
        // Arrange
        string value = "Operating Room";
        var roomType = new RoomType(value);
        var expectedString = "Operating Room";

        // Act
        var result = roomType.ToString();

        // Assert
        Assert.Equal(expectedString, result);
    }
}