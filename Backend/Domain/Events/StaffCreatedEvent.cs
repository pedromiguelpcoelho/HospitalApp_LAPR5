using System;
using DDDSample1.Domain.StaffProfile;

namespace DDDSample1.Domain.Events
{
    public class StaffCreatedEvent
    {
        public Guid StaffId { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Name { get; private set; }
        public string Role { get; private set; }
        public string LicenseNumber { get; private set; }
        public string Specialization { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public StaffCreatedEvent(Guid staffId,string firstName,string lastName, string name, string role,string licenseNumber, string specialization, string email, string phoneNumber)
        {
            StaffId = staffId;
            FirstName = firstName;
            LastName = lastName;
            Name = firstName + " " + lastName;
            Role = role;
            LicenseNumber = licenseNumber;
            Specialization = specialization;
            Email = email;
            PhoneNumber = phoneNumber;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
