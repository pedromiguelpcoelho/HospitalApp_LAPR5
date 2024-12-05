using System;
using Xunit;

namespace DomainTests
{
    public class LicenseNumberTest
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenRoleIsNullOrEmpty()
        {
            // Testa se ArgumentException é lançado quando o papel é nulo ou vazio
            Assert.Throws<ArgumentException>(() => new LicenseNumber(null, 1));
            Assert.Throws<ArgumentException>(() => new LicenseNumber("", 1));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenSequentialNumberIsZeroOrNegative()
        {
            // Testa se ArgumentException é lançado para números sequenciais inválidos (zero ou negativo)
            Assert.Throws<ArgumentException>(() => new LicenseNumber("Doctor", 0));
            Assert.Throws<ArgumentException>(() => new LicenseNumber("Nurse", -5));
        }

        [Fact]
        public void Constructor_ShouldSetLicenseNumberValue_WhenInputIsValid()
        {
            // Verifica se o número de licença é corretamente setado com valores válidos
            var currentYear = DateTime.Now.Year;
            var licenseNumber = new LicenseNumber("Doctor", 123);
            Assert.Equal($"D{currentYear}00123", licenseNumber.Value);
        }

        [Fact]
        public void GenerateLicenseNumber_ShouldUseCorrectLicenseTypeBasedOnRole()
        {
            // Testa se o tipo de licença correto é atribuído com base no papel
            var currentYear = DateTime.Now.Year;

            var doctorLicense = new LicenseNumber("Doctor", 10);
            var nurseLicense = new LicenseNumber("Nurse", 15);
            var otherLicense = new LicenseNumber("Other", 20);

            Assert.Equal($"D{currentYear}00010", doctorLicense.Value);
            Assert.Equal($"N{currentYear}00015", nurseLicense.Value);
            Assert.Equal($"O{currentYear}00020", otherLicense.Value);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertToString()
        {
            // Verifica se a conversão implícita para string funciona corretamente
            var currentYear = DateTime.Now.Year;
            var licenseNumber = new LicenseNumber("Doctor", 123);
            string licenseNumberString = licenseNumber;
            Assert.Equal($"D{currentYear}00123", licenseNumberString);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertFromTuple()
        {
            // Verifica se a conversão implícita de (string role, int sequentialNumber) para LicenseNumber funciona
            var currentYear = DateTime.Now.Year;
            LicenseNumber licenseNumber = ("Doctor", 123);
            Assert.Equal($"D{currentYear}00123", licenseNumber.Value);
        }

        [Fact]
        public void ToString_ShouldReturnLicenseNumberValue()
        {
            // Verifica se o método ToString retorna o valor correto
            var currentYear = DateTime.Now.Year;
            var licenseNumber = new LicenseNumber("Nurse", 456);
            Assert.Equal($"N{currentYear}00456", licenseNumber.ToString());
        }
    }
}
