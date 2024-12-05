using System;

namespace DDDSample1.Domain.StaffProfile
{
    public class CreatingStaffDto
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Name of the staff member
        public string Name { get; set; }

        // Role of the staff member (e.g., "Doctor", "Nurse", "Other")
        public string Role { get; set; }

        public string LicenseNumber { get; set; }

        // Specialization of the staff member (e.g., "Orthopedic Surgeon", "Anesthesiologist")
        public string Specialization { get; set; }

        // Email of the staff member
        public string Email { get; set; }

        // Phone number of the staff member
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        // Constructor to initialize the CreatingStaffDto
        public CreatingStaffDto(string firstname,string lastname, string role, string specialization, string email, string phoneNumber)
        {

            this.FirstName = firstname;
            this.LastName = lastname;
            this.Name = firstname + " "+ lastname;
            this.Role = role;
            this.Specialization = specialization;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.IsActive = true;
        }
    }
}

