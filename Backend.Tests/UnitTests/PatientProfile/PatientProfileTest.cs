using System;
using Xunit;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;

namespace DefaultNamespace
{
    public class PatientProfileTest
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange
            var firstName = new FirstName("João");
            var lastName = new LastName("Sousa");
            var fullName = new FullName("João Sousa");
            var dateOfBirth = new DateTime(1990, 1, 1);
            var email = new Email("joao.sousa@example.com");
            var contactInformation = new ContactInformation(966555444);

            // Act
            var patientProfile = new PatientProfile(firstName, lastName, fullName, dateOfBirth, email, contactInformation);

            // Assert
            Assert.Equal(firstName, patientProfile.FirstName);
            Assert.Equal(lastName, patientProfile.LastName);
            Assert.Equal(fullName, patientProfile.FullName);
            Assert.Equal(dateOfBirth, patientProfile.DateOfBirth);
            Assert.Equal(email, patientProfile.Email);
            Assert.Equal(contactInformation, patientProfile.ContactInformation);
            Assert.NotNull(patientProfile.MedicalRecordNumber);
        }

        [Fact]
        public void UpdateDetails_ShouldUpdateProperties()
        {
            // Arrange
            var patientProfile = new PatientProfile(
                new FirstName("João"),
                new LastName("Sousa"),
                new FullName("João Sousa"),
                new DateTime(1990, 1, 1),
                new Email("joao.sousa@example.com"),
                new ContactInformation(966555444)
            );

            var newFirstName = new FirstName("Jane");
            var newLastName = new LastName("Smith");
            var newFullName = new FullName("Jane Smith");
            var newDateOfBirth = new DateTime(1991, 2, 2);
            var newEmail = new Email("jane.smith@example.com");
            var newContactInformation = new ContactInformation(987654321);

            // Act
            patientProfile.UpdateDetails(newFirstName, newLastName, newFullName, newDateOfBirth, newEmail, newContactInformation);

            // Assert
            Assert.Equal(newFirstName, patientProfile.FirstName);
            Assert.Equal(newLastName, patientProfile.LastName);
            Assert.Equal(newFullName, patientProfile.FullName);
            Assert.Equal(newDateOfBirth, patientProfile.DateOfBirth);
            Assert.Equal(newEmail, patientProfile.Email);
            Assert.Equal(newContactInformation, patientProfile.ContactInformation);
        }

        [Fact]
        public void AppendToAppointmentHistory_ShouldAddEntry()
        {
            // Arrange
            var patientProfile = new PatientProfile(
                new FirstName("João"),
                new LastName("Sousa"),
                new FullName("João Sousa"),
                new DateTime(1990, 1, 1),
                new Email("joao.sousa@example.com"),
                new ContactInformation(966555444)
            );

            var operationTypeName = "Test Operation";
            var suggestedDeadline = new DateTime(2023, 10, 1);

            // Act
            patientProfile.AppendToAppointmentHistory(operationTypeName, suggestedDeadline);

            // Assert
            Assert.Equal($"Operation: {operationTypeName}, Date: {suggestedDeadline.ToShortDateString()}", patientProfile.AppointmentHistory);
        }

        [Fact]
        public void RemoveFromAppointmentHistory_ShouldRemoveEntry()
        {
            // Arrange
            var patientProfile = new PatientProfile(
                new FirstName("João"),
                new LastName("Sousa"),
                new FullName("João Sousa"),
                new DateTime(1990, 1, 1),
                new Email("joao.sousa@example.com"),
                new ContactInformation(966555444)
            );

            var operationTypeName = "Test Operation";
            var suggestedDeadline = new DateTime(2023, 10, 1);
            patientProfile.AppendToAppointmentHistory(operationTypeName, suggestedDeadline);

            // Act
            patientProfile.RemoveFromAppointmentHistory(operationTypeName, suggestedDeadline);

            // Assert
            Assert.Null(patientProfile.AppointmentHistory);
        }
    }
}