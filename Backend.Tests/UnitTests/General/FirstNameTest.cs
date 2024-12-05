using System;
using Xunit;
using DDDSample1.Domain.Shared;

namespace DefaultNamespace
{
    public class FirstNameTest
    {
        [Fact]
        public void Constructor_ShouldThrowBusinessRuleValidationException_WhenFirstNameIsNullOrEmpty()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new FirstName(null));
            Assert.Throws<BusinessRuleValidationException>(() => new FirstName(string.Empty));
        }

        [Fact]
        public void Constructor_ShouldSetFirstNameValue_WhenFirstNameIsValid()
        {
            var firstName = new FirstName("João");
            Assert.Equal("João", firstName.Value);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertToString()
        {
            var firstName = new FirstName("João");
            string firstNameString = firstName;
            Assert.Equal("João", firstNameString);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertFromString()
        {
            FirstName firstName = "João";
            Assert.Equal("João", firstName.Value);
        }

        [Fact]
        public void ToString_ShouldReturnFirstNameValue()
        {
            var firstName = new FirstName("João");
            Assert.Equal("João", firstName.ToString());
        }
    }
}