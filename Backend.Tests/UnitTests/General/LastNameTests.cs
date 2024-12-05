using System;
using Xunit;
using DDDSample1.Domain.Shared;

namespace DefaultNamespace
{
    public class LastNameTest
    {
        [Fact]
        public void Constructor_ShouldThrowBusinessRuleValidationException_WhenLastNameIsNullOrEmpty()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new LastName(null));
            Assert.Throws<BusinessRuleValidationException>(() => new LastName(string.Empty));
        }

        [Fact]
        public void Constructor_ShouldSetLastNameValue_WhenLastNameIsValid()
        {
            var lastName = new LastName("Sousa");
            Assert.Equal("Sousa", lastName.Value);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertToString()
        {
            var lastName = new LastName("Sousa");
            string lastNameString = lastName;
            Assert.Equal("Sousa", lastNameString);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertFromString()
        {
            LastName lastName = "Sousa";
            Assert.Equal("Sousa", lastName.Value);
        }

        [Fact]
        public void ToString_ShouldReturnLastNameValue()
        {
            var lastName = new LastName("Sousa");
            Assert.Equal("Sousa", lastName.ToString());
        }
    }
}