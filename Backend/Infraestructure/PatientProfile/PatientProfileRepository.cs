using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Patients;
using DDDSample1.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Infrastructure.Patients{
    public class PatientProfileRepository : BaseRepository<PatientProfile, PatientProfileId>, IPatientProfileRepository {
        private readonly DDDSample1DbContext _context;

        public PatientProfileRepository(DDDSample1DbContext context) : base(context.PatientProfiles) {
            _context = context;
        }

        public async Task<PatientProfile> FindByEmailAndExcludedIdAsync(string email, PatientProfileId excludedId) {
            return await _context.PatientProfiles
                .FirstOrDefaultAsync(op => op.Email == email && op.Id != excludedId);
        }

        public async Task<PatientProfile> FindByEmailAsync(string email) {
            return await _context.PatientProfiles
                .FirstOrDefaultAsync(p => p.Email == email);
            
        }

        public async Task<PatientProfile> FindByMedicalRecordNumberAsync(string medicalRecordNumber) {
            return await _context.PatientProfiles
                .FirstOrDefaultAsync(p => p.MedicalRecordNumber == medicalRecordNumber);
        }

        public async Task<PatientProfile> FindByFullNameAsync(string fullName)
        {
            return await _context.PatientProfiles
                .FirstOrDefaultAsync(p => p.FullName == fullName);
        }

        public async Task<List<PatientProfile>> FindByNameAsync(string firstName, string lastName)
        {
            return await _context.PatientProfiles
                .Where(p => p.FirstName == firstName && p.LastName == lastName)
                .ToListAsync();        
        }

        public async Task<PatientProfile> FindByDateOfBirthAsync(DateTime dateOfBirth)
        {
            return await _context.PatientProfiles
                .FirstOrDefaultAsync(p => p.DateOfBirth == dateOfBirth);
        }

        public async Task<PatientProfile> FindByPhoneNumberAsync(long phoneNumber)
        {
            return await _context.PatientProfiles
                .FirstOrDefaultAsync(p => p.ContactInformation == phoneNumber);
        }

        public async Task<bool> ExistsAsync(string email) {
            return await _context.PatientProfiles.AnyAsync(p => p.Email == email);
        }

        public async Task<IEnumerable<PatientProfile>> GetPatientsMarkedForDeletionAsync() {
            return await _context.PatientProfiles
                .Where(p => p.IsMarkedForDeletion)
                .ToListAsync();
        }
    }
}