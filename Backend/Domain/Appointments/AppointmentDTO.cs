using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.Appointments
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public Guid RequestId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan Duration { get; set; }
        public AppointmentStatus Status { get; set; }
        public List<Guid> AssignedStaff { get; set; } 
    }
}

