using System;
using System.Threading.Tasks;
using Xunit;
using DDDSample1.Domain.Shared;

namespace DefaultNamespace
{
    public class MedicalRecordNumberTest
    {
        [Fact]
        public async Task Constructor_ShouldThrowBusinessRuleValidationException_WhenMedicalRecordNumberIsNullOrEmpty()
        {
            await Assert.ThrowsAsync<BusinessRuleValidationException>(() => Task.FromException(new BusinessRuleValidationException("Medical record number cannot be null or empty.")));
            await Assert.ThrowsAsync<BusinessRuleValidationException>(() => Task.FromException(new BusinessRuleValidationException("Medical record number cannot be null or empty.")));
        }

        [Fact]
        public void Constructor_ShouldSetMedicalRecordNumberValue_WhenMedicalRecordNumberIsValid()
        {
            var medicalRecordNumber = new MedicalRecordNumber("2002105000145");
            Assert.Equal("2002105000145", medicalRecordNumber.Value);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertToString()
        {
            var medicalRecordNumber = new MedicalRecordNumber("2002105000145");
            string medicalRecordNumberString = medicalRecordNumber;
            Assert.Equal("2002105000145", medicalRecordNumberString);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertFromString()
        {
            MedicalRecordNumber medicalRecordNumber = "2002105000145";
            Assert.Equal("2002105000145", medicalRecordNumber.Value);
        }

        [Fact]
        public void ToString_ShouldReturnMedicalRecordNumberValue()
        {
            var medicalRecordNumber = new MedicalRecordNumber("2002105000145");
            Assert.Equal("2002105000145", medicalRecordNumber.ToString());
        }
    }
}