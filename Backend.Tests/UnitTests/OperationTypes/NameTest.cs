using System;
using DDDSample1.Domain.Shared;
using Xunit;

public class NameTests
{
    [Fact]
    public void Constructor_ShouldCreateName_WhenValidValue()
    {
        // Arrange
        var value = "Valid Name";

        // Act
        var name = new Name(value);

        // Assert
        Assert.Equal(value, name.Value);
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleValidationException_WhenValueIsNull()
    {
        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => new Name(null));
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleValidationException_WhenValueIsEmpty()
    {
        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => new Name(string.Empty));
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleValidationException_WhenValueIsWhitespace()
    {
        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() => new Name("   "));
    }

    [Fact]
    public void ImplicitConversion_ShouldConvertStringToName()
    {
        // Arrange
        var value = "Valid Name";

        // Act
        Name name = value;

        // Assert
        Assert.Equal(value, name.Value);
    }

    [Fact]
    public void ImplicitConversion_ShouldConvertNameToString()
    {
        // Arrange
        var value = "Valid Name";
        var name = new Name(value);

        // Act
        string stringValue = name;

        // Assert
        Assert.Equal(value, stringValue);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var value = "Valid Name";
        var name = new Name(value);

        // Act
        var result = name.ToString();

        // Assert
        Assert.Equal(value, result);
    }
}