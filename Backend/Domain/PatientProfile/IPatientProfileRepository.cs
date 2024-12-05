using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Patients {
    public interface IPatientProfileRepository : IRepository<PatientProfile, PatientProfileId> {
        Task<PatientProfile> FindByEmailAndExcludedIdAsync(string email, PatientProfileId excludedId);
        Task<PatientProfile> FindByEmailAsync(string email);
        Task<PatientProfile> FindByMedicalRecordNumberAsync(string medicalRecordNumber);
        Task<PatientProfile> FindByFullNameAsync(string fullName);
        Task<PatientProfile> FindByDateOfBirthAsync (DateTime dateOfBirth);   
        Task<PatientProfile> FindByPhoneNumberAsync (long phoneNumber);
        Task<bool> ExistsAsync(string email);
        Task<IEnumerable<PatientProfile>> GetPatientsMarkedForDeletionAsync();
    }
}