using System;
using System.Collections.Generic;
using DDDSample1.Domain.Shared;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace DDDSample1.Domain.StaffProfile
{
    public class Staff : Entity<StaffId>, IAggregateRoot
    {
        public FirstName FirstName { get; set; }
        public LastName LastName { get; set; }

        // Name of the staff member
        public FullName Name { get; private set; }

        public Role Role { get; private set; }

        public LicenseNumber LicenseNumber { get; private set; }

        // Specialization of the staff member (e.g., "Orthopedic Surgeon", "Anesthesiologist")
        public Specialization Specialization { get; private set; }

        // Contact information (e.g., email, phone number)
        public Email Email { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; }

        // Boolean flag indicating whether the staff member is active
        public bool IsActive { get; private set; }


        // Static variable to track the last sequential number used
        private static int sequentialNumber = 0;

        private static List<AvailabilitySlot> slots;

        public Staff() 
    {
        // Inicialize as propriedades se necessário
    }

        // Constructor to initialize the Staff profile
        public Staff(string firstname,string lastName ,string role, string specialization, string email, string phoneNumber)
        {
            this.Id = new StaffId(Guid.NewGuid());
            this.FirstName = new FirstName(firstname);
            this.LastName = new LastName(lastName);
            this.Name = new FullName(firstname + " "+ lastName);
            this.Role = new Role(role);
            this.Specialization = new Specialization(specialization,role);
            this.Email = new Email(email);
            this.PhoneNumber = new PhoneNumber(phoneNumber);

            this.IsActive = true;

            sequentialNumber++;
            this.LicenseNumber = new LicenseNumber(role, sequentialNumber);
    
             
        }

        // Method to update staff details
        public void UpdateDetails(string firstName,string lastName,string role ,string specialization, string email, string phoneNumber)
        {

            this.FirstName = new FirstName(firstName);
            this.LastName = new LastName(lastName);
            this.Name = new FullName(firstName +" "+ lastName);
            this.Role = new Role(role);
            this.Specialization = new Specialization(specialization,role);
            this.LicenseNumber = new LicenseNumber(role,sequentialNumber);
            this.Email = new Email(email);
            this.PhoneNumber = new PhoneNumber(phoneNumber);
        }

         
        public void UpdateFirstName(FirstName newFirstName)
        {
            if (newFirstName == null)
                throw new ArgumentNullException(nameof(newFirstName));

            FirstName = newFirstName;
        }

        public void UpdateLastName(LastName newLastName) => LastName = newLastName ?? throw new ArgumentNullException(nameof(newLastName));
        public void UpdateEmail(Email newEmail) => Email = newEmail ?? throw new ArgumentNullException(nameof(newEmail));
        public void UpdatePhoneNumber(PhoneNumber newPhoneNumber) => PhoneNumber = newPhoneNumber ?? throw new ArgumentNullException(nameof(newPhoneNumber));
        public void UpdateRole(Role newRole) => Role = newRole ?? throw new ArgumentNullException(nameof(newRole));
        public void UpdateSpecialization(Specialization newSpecialization) => Specialization = newSpecialization ?? throw new ArgumentNullException(nameof(newSpecialization));


        // Method to deactivate a staff member
        public void Deactivate()
        {
            this.IsActive = false;
        }


    }
}
