using System;
using Xunit;

namespace DefaultNamespace
{
    public class EmailTest
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenEmailIsNullOrEmpty()
        {
            Assert.Throws<ArgumentException>(() => new Email(null));
            Assert.Throws<ArgumentException>(() => new Email(string.Empty));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenEmailIsInvalid()
        {
            Assert.Throws<ArgumentException>(() => new Email("invalidemail")); // Missing '@' and '.'
            Assert.Throws<ArgumentException>(() => new Email("invalid@domain")); // Missing '.'
            Assert.Throws<ArgumentException>(() => new Email("invalid.com")); // Missing '@'
        }

        [Fact]
        public void Constructor_ShouldSetEmailValue_WhenEmailIsValid()
        {
            var email = new Email("test@example.com");
            Assert.Equal("test@example.com", email.Value);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertToString()
        {
            var email = new Email("test@example.com");
            string emailString = email;
            Assert.Equal("test@example.com", emailString);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertFromString()
        {
            Email email = "test@example.com";
            Assert.Equal("test@example.com", email.Value);
        }

        [Fact]
        public void ToString_ShouldReturnEmailValue()
        {
            var email = new Email("test@example.com");
            Assert.Equal("test@example.com", email.ToString());
        }
    }
}