using System;

namespace DDDSample1.Domain.StaffProfile
{
    public class StaffDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string LicenseNumber { get; set; }
        public string Specialization { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }

    
        
    }
}
