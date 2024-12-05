using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using System.Collections.Generic;

namespace DDDSample1.Domain.StaffProfile
{
    public interface IStaffRepository : IRepository<Staff, StaffId>
    {

         // Method to find a staff member by phone number
        Task<Staff> GetByPhoneNumberAsync(string phoneNumber);
        
        // Method to find a staff member by email
        Task<Staff> GetByEmailAsync(string email);

        // Method to find a staff member by license number, excluding a specific StaffId
        Task<Staff> FindByLicenseNumberAndExcludedIdAsync(string licenseNumber, StaffId excludedId);
        
        // Method to find a staff member by email, excluding a specific StaffId
        Task<Staff> FindByEmailAndExcludedIdAsync(string email, StaffId excludedId);

        // Method to search staff profiles by name, specialization, license number, and active status
        Task<List<Staff>> SearchStaffProfilesAsync(string name,string email ,string phoneNumber,string specialization, string licenseNumber ,  bool? isActive = null );
  

    }
}
