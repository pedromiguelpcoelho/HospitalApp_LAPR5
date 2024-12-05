using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.StaffProfile;
using DDDSample1.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Infrastructure.StaffProfile
{
    public class StaffRepository : BaseRepository<Staff, StaffId>, IStaffRepository
    {
        private readonly DDDSample1DbContext _context;

        public StaffRepository(DDDSample1DbContext context) : base(context.Staffs)
        {
            _context = context;
        }


         // Find staff by phone number
        public async Task<Staff> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Staffs
                .FirstOrDefaultAsync(s => s.PhoneNumber == phoneNumber);
        }

        // Find staff by email
        public async Task<Staff> GetByEmailAsync(string email)
        {
            return await _context.Staffs
                .FirstOrDefaultAsync(s => s.Email == email);
        }


        // Find staff by license number excluding a specific staff ID
        public async Task<Staff> FindByLicenseNumberAndExcludedIdAsync(string licenseNumber, StaffId excludedId)
        {
            return await _context.Staffs
                .FirstOrDefaultAsync(s => s.LicenseNumber == licenseNumber && s.Id != excludedId);
        }

        // Find staff by email excluding a specific staff ID
        public async Task<Staff> FindByEmailAndExcludedIdAsync(string email, StaffId excludedId)
        {
            return await _context.Staffs
                .FirstOrDefaultAsync(s => s.Email == email && s.Id != excludedId);
        }

       
       public async Task<List<Staff>> SearchStaffProfilesAsync(string name, string email,string phoneNumber,string specialization, string licenseNumber,  bool? isActive = null)
        {
            var query = _context.Staffs.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                
                var staffIds = _context.Staffs
                .AsEnumerable() 
                .Where(s => s.Name.Value.Contains(name, StringComparison.OrdinalIgnoreCase)) 
                .Select(s => s.Id) 
                .ToList(); 

                
                query = query.Where(or => staffIds.Contains(or.Id));
            }

            if (!string.IsNullOrEmpty(specialization))
            {

            var staffIds = _context.Staffs
            .AsEnumerable() 
            .Where(s => s.Specialization.ToString() == specialization) 
            .Select(s => s.Id) 
            .ToList(); 

            query = query.Where(or => staffIds.Contains(or.Id));
            }

           if (!string.IsNullOrEmpty(licenseNumber))
            {

            var staffIds = _context.Staffs
            .AsEnumerable() 
            .Where(s => s.LicenseNumber.ToString() == licenseNumber) 
            .Select(s => s.Id) 
            .ToList(); 

            query = query.Where(or => staffIds.Contains(or.Id));
            }


             if (!string.IsNullOrEmpty(email))
            {

            var staffIds = _context.Staffs
            .AsEnumerable() 
            .Where(s => s.Email.ToString() == email) 
            .Select(s => s.Id) 
            .ToList(); 

            query = query.Where(or => staffIds.Contains(or.Id));
            }


             if (!string.IsNullOrEmpty(phoneNumber))
            {

            var staffIds = _context.Staffs
            .AsEnumerable() 
            .Where(s => s.PhoneNumber.ToString() == phoneNumber) 
            .Select(s => s.Id) 
            .ToList(); 

            query = query.Where(or => staffIds.Contains(or.Id));
            }

            if (isActive.HasValue)
            {
                query = query.Where(s => s.IsActive == isActive.Value);
            }

            return await query.ToListAsync();
        }
    }
}
