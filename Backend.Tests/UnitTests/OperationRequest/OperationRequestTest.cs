using System;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.StaffProfile;
using Moq;
using Xunit;

namespace DefaultNamespace
{
    public class OperationRequestTest
    {
        
        [Fact]
        public void Constructor_ShouldCreateOperationRequest_WhenInputsAreValid()
        {
            // Arrange
            var patientId = new Mock<PatientProfileId>().Object;
            var doctorId = new Mock<StaffId>().Object;
            var operationTypeId = new Mock<OperationTypeId>().Object;
            var priority = Priority.Urgent;
            var suggestedDeadline = DateTime.UtcNow.AddDays(7);

            // Act
            var operationRequest = new OperationRequest(patientId, doctorId, operationTypeId, priority, suggestedDeadline);

            // Assert
            Assert.NotNull(operationRequest);
            Assert.Equal(patientId, operationRequest.PatientId);
            Assert.Equal(doctorId, operationRequest.DoctorId);
            Assert.Equal(operationTypeId, operationRequest.OperationTypeId);
            Assert.Equal(priority, operationRequest.Priority);
            Assert.Equal(suggestedDeadline, operationRequest.SuggestedDeadline);
        }

        [Fact]
        public void UpdatePriority_ShouldUpdatePriority_WhenPriorityIsValid()
        {
            // Arrange
            var patientId = new Mock<PatientProfileId>().Object;
            var doctorId = new Mock<StaffId>().Object;
            var operationTypeId = new Mock<OperationTypeId>().Object;
            var priority = Priority.Urgent;
            var suggestedDeadline = DateTime.UtcNow.AddDays(7);
            var operationRequest = new OperationRequest(patientId, doctorId, operationTypeId, priority, suggestedDeadline);
            var newPriority = Priority.Emergency;

            // Act
            operationRequest.UpdatePriority(newPriority);

            // Assert
            Assert.Equal(newPriority, operationRequest.Priority);
        }

        [Fact]
        public void UpdateSuggestedDeadline_ShouldUpdateSuggestedDeadline_WhenDateIsValid()
        {
            // Arrange
            var patientId = new Mock<PatientProfileId>().Object;
            var doctorId = new Mock<StaffId>().Object;
            var operationTypeId = new Mock<OperationTypeId>().Object;
            var priority = Priority.Urgent;
            var suggestedDeadline = DateTime.UtcNow.AddDays(7);
            var operationRequest = new OperationRequest(patientId, doctorId, operationTypeId, priority, suggestedDeadline);
            var newDeadline = DateTime.UtcNow.AddDays(10);

            // Act
            operationRequest.UpdateSuggestedDeadline(newDeadline);

            // Assert
            Assert.Equal(newDeadline, operationRequest.SuggestedDeadline);
        }
        
        [Fact]
        public void Constructor_ShouldThrowException_WhenPatientProfileIdIsInvalid()
        {
            // Arrange
            var doctorId = new Mock<StaffId>().Object;
            var operationTypeId = new Mock<OperationTypeId>().Object;
            var priority = Priority.Urgent;
            var suggestedDeadline = DateTime.UtcNow.AddDays(7);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new OperationRequest(null, doctorId, operationTypeId, priority, suggestedDeadline));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenStaffIdIsInvalid()
        {
            // Arrange
            var patientId = new Mock<PatientProfileId>().Object;
            var operationTypeId = new Mock<OperationTypeId>().Object;
            var priority = Priority.Urgent;
            var suggestedDeadline = DateTime.UtcNow.AddDays(7);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new OperationRequest(patientId, null, operationTypeId, priority, suggestedDeadline));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenOperationTypeIdIsInvalid()
        {
            // Arrange
            var patientId = new Mock<PatientProfileId>().Object;
            var doctorId = new Mock<StaffId>().Object;
            var priority = Priority.Urgent;
            var suggestedDeadline = DateTime.UtcNow.AddDays(7);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new OperationRequest(patientId, doctorId, null, priority, suggestedDeadline));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenSuggestedDeadlineIsInThePast()
        {
            // Arrange
            var patientId = new Mock<PatientProfileId>().Object;
            var doctorId = new Mock<StaffId>().Object;
            var operationTypeId = new Mock<OperationTypeId>().Object;
            var priority = Priority.Urgent;
            var pastDeadline = DateTime.UtcNow.AddDays(-1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new OperationRequest(patientId, doctorId, operationTypeId, priority, pastDeadline));
        }

        [Fact]
        public void UpdatePriority_ShouldThrowException_WhenPriorityIsInvalid()
        {
            // Arrange
            var patientId = new Mock<PatientProfileId>().Object;
            var doctorId = new Mock<StaffId>().Object;
            var operationTypeId = new Mock<OperationTypeId>().Object;
            var priority = Priority.Urgent;
            var suggestedDeadline = DateTime.UtcNow.AddDays(7);
            var operationRequest = new OperationRequest(patientId, doctorId, operationTypeId, priority, suggestedDeadline);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => operationRequest.UpdatePriority((Priority)999));
        }

        [Fact]
        public void UpdateSuggestedDeadline_ShouldThrowException_WhenDateIsInThePast()
        {
            // Arrange
            var patientId = new Mock<PatientProfileId>().Object;
            var doctorId = new Mock<StaffId>().Object;
            var operationTypeId = new Mock<OperationTypeId>().Object;
            var priority = Priority.Urgent;
            var suggestedDeadline = DateTime.UtcNow.AddDays(7);
            var operationRequest = new OperationRequest(patientId, doctorId, operationTypeId, priority, suggestedDeadline);
            var pastDeadline = DateTime.UtcNow.AddDays(-1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => operationRequest.UpdateSuggestedDeadline(pastDeadline));
        }
    }
}