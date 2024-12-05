using System;
using System.Collections.Generic;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.StaffProfile;
using DDDSample1.Domain.SurgeryRooms;

namespace DDDSample1.Domain.Appointments
{
    public class Appointment : Entity<AppointmentId>, IAggregateRoot
    {
        public OperationRequestId RequestId { get; private set; }
        public SurgeryRoomId RoomId { get; private set; }
        public DateTime StartTime { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public List<StaffId> AssignedStaff { get; private set; }
        public TimeSpan Duration { get; private set; }

        
        protected Appointment() { }

        public Appointment(OperationRequestId requestId, SurgeryRoomId roomId, DateTime startTime, TimeSpan duration, AppointmentStatus status, List<StaffId> assignedStaff)
        {
            this.Id = new AppointmentId(Guid.NewGuid());
            this.RequestId = requestId ?? throw new ArgumentNullException(nameof(requestId));
            this.RoomId = roomId ?? throw new ArgumentNullException(nameof(roomId));
            this.StartTime = startTime;
            this.Duration = duration;
            this.Status = status;
            this.AssignedStaff = assignedStaff ?? throw new ArgumentNullException(nameof(assignedStaff));
        }

        public void UpdateDetails(DateTime newDateTime, SurgeryRoomId newRoomId, List<StaffId> newAssignedStaff, TimeSpan newDuration)
        {
            if (newDateTime < DateTime.UtcNow)
                throw new ArgumentException("The appointment date cannot be in the past.");
            
            this.StartTime = newDateTime;
            this.RoomId = newRoomId;
            this.AssignedStaff = newAssignedStaff;
            this.Duration = newDuration;
        }

        public void ChangeStatus(AppointmentStatus newStatus)
        {
            this.Status = newStatus;
        }
    }


}
