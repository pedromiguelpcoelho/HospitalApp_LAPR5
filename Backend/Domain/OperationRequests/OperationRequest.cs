using System;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.StaffProfile;

namespace DDDSample1.Domain.OperationRequests
{
    public class OperationRequest : Entity<OperationRequestId>, IAggregateRoot
    {
        public PatientProfileId PatientId { get; private set; }
        public StaffId DoctorId { get; private set; }
        public OperationTypeId OperationTypeId { get; private set; }
        public Priority Priority { get; private set; }
        public DateTime SuggestedDeadline { get; private set; }
        public DateTime RequestDate { get; private set; }

        // Private constructor for ORM (e.g., EF Core)
        private OperationRequest() { }

        // Main Constructor
        public OperationRequest(PatientProfileId patientId, StaffId doctorId, OperationTypeId operationTypeId, Priority priority, DateTime suggestedDeadline)
        {
            this.PatientId = patientId ?? throw new ArgumentNullException(nameof(patientId), "Patient ID cannot be null.");
            this.DoctorId = doctorId ?? throw new ArgumentNullException(nameof(doctorId), "Doctor ID cannot be null.");
            this.OperationTypeId = operationTypeId ?? throw new ArgumentNullException(nameof(operationTypeId), "Operation Type ID cannot be null.");
            this.Priority = priority;
            this.SuggestedDeadline = suggestedDeadline > DateTime.UtcNow ? suggestedDeadline : throw new ArgumentException("Suggested deadline cannot be in the past.", nameof(suggestedDeadline));
            this.RequestDate = DateTime.UtcNow;
            this.Id = new OperationRequestId(Guid.NewGuid()); 
        }

        public void UpdatePriority(Priority priority)
        {
            if (!Enum.IsDefined(typeof(Priority), priority))
                throw new ArgumentException("Invalid priority value", nameof(priority));

            Priority = priority;
        }

        public void UpdateSuggestedDeadline(DateTime suggestedDeadline)
        {
            if (suggestedDeadline < DateTime.UtcNow)
                throw new ArgumentException("Suggested deadline cannot be in the past", nameof(suggestedDeadline));

            this.SuggestedDeadline = suggestedDeadline;
        }
    }
}