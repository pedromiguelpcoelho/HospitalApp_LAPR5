using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.Appointments
{
    public class CreatingAppointmentDto
    {
        public CreatingAppointmentDto(
            Guid requestId,
            Guid roomId,
            DateTime dateTime,
            TimeSpan duration,
            AppointmentStatus status,
            List<Guid> assignedStaff)
        {
            RequestId = requestId;
            RoomId = roomId;
            DateTime = dateTime;
            Duration = duration;
            Status = status;
            AssignedStaff = assignedStaff;
        }

        public Guid RequestId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan Duration { get; set; }
        public AppointmentStatus Status { get; set; }
        public List<Guid> AssignedStaff { get; set; } 
    }
}
