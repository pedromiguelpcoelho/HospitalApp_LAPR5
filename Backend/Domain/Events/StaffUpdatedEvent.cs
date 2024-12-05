using System;
using DDDSample1.Domain.StaffProfile;

namespace DDDSample1.Domain.Events
{
    public class StaffUpdatedEvent
    {
        public Guid StaffId { get; private set; }
        public FullName FullName { get; private set; }
        public Role Role { get; private set; }
        public Specialization Specialization { get; private set; }
        public Email Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public StaffUpdatedEvent(
            Guid staffId,
            FullName fullName,
            Role role,
            Specialization specialization,
            Email email,
            PhoneNumber phoneNumber
        )
        {
            StaffId = staffId;
            FullName = fullName;
            Role = role;
            Specialization = specialization;
            Email = email;
            PhoneNumber = phoneNumber;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
