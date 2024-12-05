using System;
using DDDSample1.Domain.StaffProfile;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DomainTests
{
    public class StaffTest
    {
        [Fact]
        public void Constructor_ShouldInitializeStaffWithValidValues()
        {
            // Testa a inicialização do staff com valores válidos
            var staff = new Staff("John", "Doe", "Doctor", "Orthopaedist", "john.doe@example.com", "912345678");
            
            Assert.Equal("John", staff.FirstName.Value);
            Assert.Equal("Doe", staff.LastName.Value);
            Assert.Equal("Doctor", staff.Role.Value);
            Assert.Equal("Orthopaedist", staff.Specialization.Value);
            Assert.Equal("john.doe@example.com", staff.Email.Value);
            Assert.Equal("912345678", staff.PhoneNumber.Value);
            Assert.True(staff.IsActive);
            Assert.NotNull(staff.LicenseNumber);
        }

        [Fact]
        public void UpdateDetails_ShouldUpdateStaffDetailsSuccessfully()
        {
            // Testa a atualização de detalhes do staff
            var staff = new Staff("John", "Doe", "Doctor", "Orthopaedist", "john.doe@example.com", "912345678");
            staff.UpdateDetails("Jane", "Smith", "Nurse", "Nurse Anaesthetist", "jane.smith@example.com", "912345679");

            Assert.Equal("Jane", staff.FirstName.Value);
            Assert.Equal("Smith", staff.LastName.Value);
            Assert.Equal("Nurse", staff.Role.Value);
            Assert.Equal("Nurse Anaesthetist", staff.Specialization.Value);
            Assert.Equal("jane.smith@example.com", staff.Email.Value);
            Assert.Equal("912345679", staff.PhoneNumber.Value);
        }

        [Fact]
        public void Deactivate_ShouldSetIsActiveToFalse()
        {
            // Testa se o método Deactivate altera o status de ativo
            var staff = new Staff("John", "Doe", "Doctor", "Orthopaedist", "john.doe@example.com", "912345678");
            staff.Deactivate();

            Assert.False(staff.IsActive);
        }
    }
}
