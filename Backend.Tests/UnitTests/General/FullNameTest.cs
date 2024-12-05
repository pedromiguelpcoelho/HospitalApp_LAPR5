using System;
using Xunit;
using DDDSample1.Domain.Shared;

namespace DefaultNamespace
{
    public class FullNameTest
    {
        [Fact]
        public void Constructor_ShouldThrowBusinessRuleValidationException_WhenFullNameIsNullOrEmpty()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new FullName(null));
            Assert.Throws<BusinessRuleValidationException>(() => new FullName(string.Empty));
        }

        [Fact]
        public void Constructor_ShouldSetFullNameValue_WhenFullNameIsValid()
        {
            var fullName = new FullName("João Carneiro Sousa");
            Assert.Equal("João Carneiro Sousa", fullName.Value);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertToString()
        {
            var fullName = new FullName("João Carneiro Sousa");
            string fullNameString = fullName;
            Assert.Equal("João Carneiro Sousa", fullNameString);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertFromString()
        {
            FullName fullName = "João Carneiro Sousa";
            Assert.Equal("João Carneiro Sousa", fullName.Value);
        }

        [Fact]
        public void ToString_ShouldReturnFullNameValue()
        {
            var fullName = new FullName("João Carneiro Sousa");
            Assert.Equal("João Carneiro Sousa", fullName.ToString());
        }
    }
}