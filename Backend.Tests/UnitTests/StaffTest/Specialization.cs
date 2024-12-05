using System;
using Xunit;

namespace DomainTests
{
    public class SpecializationTest
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenSpecializationIsNullOrEmpty()
        {
            // Testa se ArgumentException é lançado quando a especialização é nula ou vazia
            Assert.Throws<ArgumentException>(() => new Specialization(null, "Doctor"));
            Assert.Throws<ArgumentException>(() => new Specialization("", "Nurse"));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenDoctorSpecializationIsInvalid()
        {
            // Testa se ArgumentException é lançado para especializações inválidas de Doctor
            Assert.Throws<ArgumentException>(() => new Specialization("General Practitioner", "Doctor"));
            Assert.Throws<ArgumentException>(() => new Specialization("Cardiologist", "Doctor"));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenNurseSpecializationIsInvalid()
        {
            // Testa se ArgumentException é lançado para especializações inválidas de Nurse
            Assert.Throws<ArgumentException>(() => new Specialization("Surgeon", "Nurse"));
            Assert.Throws<ArgumentException>(() => new Specialization("Pediatrician", "Nurse"));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenOtherSpecializationIsInvalid()
        {
            // Testa se ArgumentException é lançado para especializações inválidas de Other
            Assert.Throws<ArgumentException>(() => new Specialization("Radiologist", "Other"));
        }

        [Fact]
        public void Constructor_ShouldSetSpecializationValue_WhenInputIsValid()
        {
            // Verifica se a especialização é corretamente setada quando o valor é válido
            var doctorSpecialization = new Specialization("Orthopaedist", "Doctor");
            Assert.Equal("Orthopaedist", doctorSpecialization.Value);

            var nurseSpecialization = new Specialization("Nurse Anaesthetist", "Nurse");
            Assert.Equal("Nurse Anaesthetist", nurseSpecialization.Value);

            var otherSpecialization = new Specialization("X-ray Technician", "Other");
            Assert.Equal("X-ray Technician", otherSpecialization.Value);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertToString()
        {
            // Verifica se a conversão implícita para string funciona corretamente
            var specialization = new Specialization("Anaesthetist", "Doctor");
            string specializationString = specialization;
            Assert.Equal("Anaesthetist", specializationString);
        }

        [Fact]
        public void ToString_ShouldReturnSpecializationValue()
        {
            // Verifica se o método ToString retorna o valor correto
            var specialization = new Specialization("Instrumenting Nurse", "Nurse");
            Assert.Equal("Instrumenting Nurse", specialization.ToString());
        }
    }
}
