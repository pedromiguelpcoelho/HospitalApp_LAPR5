using System;
using Xunit;

namespace DefaultNamespace
{
    public class ContactInformationTest
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenPhoneNumberIsInvalid()
        {
            Assert.Throws<ArgumentException>(() => new ContactInformation(12345678)); // Less than 9 digits
            Assert.Throws<ArgumentException>(() => new ContactInformation(812345678)); // Does not start with 9
            Assert.Throws<ArgumentException>(() => new ContactInformation(9123456789)); // More than 9 digits
        }

        [Fact]
        public void Constructor_ShouldSetPhoneNumberValue_WhenPhoneNumberIsValid()
        {
            var contactInformation = new ContactInformation(912345678);
            Assert.Equal(912345678, contactInformation.Value);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertToLong()
        {
            var contactInformation = new ContactInformation(912345678);
            long phoneNumber = contactInformation;
            Assert.Equal(912345678, phoneNumber);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertFromLong()
        {
            ContactInformation contactInformation = 912345678;
            Assert.Equal(912345678, contactInformation.Value);
        }

        [Fact]
        public void ToString_ShouldReturnPhoneNumberValue()
        {
            var contactInformation = new ContactInformation(912345678);
            Assert.Equal("912345678", contactInformation.ToString());
        }
    }
}