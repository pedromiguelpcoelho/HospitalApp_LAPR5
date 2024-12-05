using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Coravel.Queuing.Interfaces;
using DDDNetCore.Domain.Email;
using DDDSample1.Domain.Events;
using DDDSample1.Domain.Events.Handlers;
using DDDSample1.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace DDDSample1.Domain.Patients
{
    public class PatientProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPatientProfileRepository _repo;
        private readonly IEmailService _emailService;
        private readonly PatientProfileCreatedEventHandler _createdEventHandler;
        private readonly PatientProfileDeletedEventHandler _deletedEventHandler;
        private readonly ITokenManager _tokenManager;
        private readonly PatientProfileUpdatedEventHandler _updatedEventHandler;
        private readonly IQueue _queue;
        private readonly ILogger<PatientProfileService> _logger;
        private readonly PatientDeletionService _deletionService;

        public PatientProfileService(IUnitOfWork unitOfWork, IPatientProfileRepository repo, IEmailService emailService,
            PatientProfileCreatedEventHandler createdEventHandler,
            PatientProfileDeletedEventHandler deletedEventHandler, ITokenManager tokenManager,
            PatientProfileUpdatedEventHandler updatedEventHandler, IQueue queue, ILogger<PatientProfileService> logger,
            PatientDeletionService deletionService)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
            _emailService = emailService;
            _createdEventHandler = createdEventHandler;
            _deletedEventHandler = deletedEventHandler;
            _tokenManager = tokenManager;
            _updatedEventHandler = updatedEventHandler;
            _queue = queue;
            _logger = logger;
            _deletionService = deletionService;
        }

        // -----------------------------------------------------------------------------------------------------------
        //ADD METHOD
        public async Task<PatientProfileDTO> AddAsync(CreatingPatientProfileDto dto) {
            if (dto == null) {
                throw new ArgumentNullException(nameof(dto));
            }

            await ValidateEmailAndContantInformation(dto.Email, dto.ContactInformation);

            var profile = new PatientProfile(new FirstName(dto.FirstName), new LastName(dto.LastName),
                new FullName(dto.FullName), dto.DateOfBirth, new Email(dto.Email),
                new ContactInformation(dto.ContactInformation));

            await this._repo.AddAsync(profile);
            await this._unitOfWork.CommitAsync();

            await _createdEventHandler.Handle(new PatientProfileCreadtedEvent(profile.Id.AsGuid(), profile.FullName,
                profile.Email));

            return ToDto(profile);
        }

        private async Task ValidateEmailAndContantInformation(string email, long contactInformation)
        {
            var existingProfileByEmail = await _repo.FindByEmailAsync(email);
            if (existingProfileByEmail != null)
            {
                throw new PatientProfileExceptions.InvalidEmailException();
            }

            var existingProfileByContact = await _repo.FindByPhoneNumberAsync(contactInformation);
            if (existingProfileByContact != null)
            {
                throw new PatientProfileExceptions.InvalidContactInformationException();
            }
        }

        //------------------------------------------------------------------------------------------------------------------
        //UPDATE METHOD - ADMIN
        
        public async Task<PatientProfileDTO> UpdateAsync(CreatingPatientProfileDto dto, string email = null,
            string medicalRecordNumber = null, string fullName = null, string dateOfBirth = null, string phoneNumber = null)
        {
            PatientProfile profile = null;

            // Find the profile based on the provided parameters
            if (!string.IsNullOrWhiteSpace(email)) {
                profile = await _repo.FindByEmailAsync(email);
                Console.WriteLine($"Profile found by email: {profile?.Id}");
            } else if (!string.IsNullOrWhiteSpace(medicalRecordNumber)) {
                profile = await _repo.FindByMedicalRecordNumberAsync(medicalRecordNumber);
                Console.WriteLine($"Profile found by medical record number: {profile?.Id}");
            } else if (!string.IsNullOrWhiteSpace(fullName)) {
                profile = await _repo.FindByFullNameAsync(fullName);
                Console.WriteLine($"Profile found by full name: {profile?.Id}");
            } else if (!string.IsNullOrWhiteSpace(dateOfBirth)) {
                profile = await _repo.FindByDateOfBirthAsync(DateTime.Parse(dateOfBirth));
                Console.WriteLine($"Profile found by date of birth: {profile?.Id}");
            } else if (!string.IsNullOrWhiteSpace(phoneNumber)) {
                profile = await _repo.FindByPhoneNumberAsync(long.Parse(phoneNumber));
                Console.WriteLine($"Profile found by phone number: {profile?.Id}");
            }

            if (profile == null) {
                Console.WriteLine("Profile not found");
                return null;
            }

            // Prepare to track changes
            var changes = new List<string>();

            // Check for unique email
            var existingProfileByEmail = await _repo.FindByEmailAndExcludedIdAsync(dto.Email, profile.Id);
            if (existingProfileByEmail != null) {
                throw new ArgumentException("A profile with the same email already exists.");
            }

            // Check for unique contact information
            var existingProfileByContact = await _repo.FindByPhoneNumberAsync(dto.ContactInformation);
            if (existingProfileByContact != null && existingProfileByContact.Id != profile.Id) {
                throw new ArgumentException("A profile with the same contact information already exists.");
            }

            // Track changes in each field
            if (profile.FirstName.Value != dto.FirstName) {
                changes.Add($"First Name: {profile.FirstName.Value} -> {dto.FirstName}");
            }
            if (profile.LastName.Value != dto.LastName) {
                changes.Add($"Last Name: {profile.LastName.Value} -> {dto.LastName}");
            }
            if (profile.FullName.Value != dto.FullName) {
                changes.Add($"Full Name: {profile.FullName.Value} -> {dto.FullName}");
            }
            if (profile.ContactInformation.Value != dto.ContactInformation) {
                changes.Add($"Contact Information: {profile.ContactInformation.Value} -> {dto.ContactInformation}");
            }
            if (profile.Email.Value != dto.Email) {
                changes.Add($"Email: {profile.Email.Value} -> {dto.Email}");
            }
            if (profile.DateOfBirth != dto.DateOfBirth) {
                changes.Add($"Date of Birth: {profile.DateOfBirth.ToShortDateString()} -> {DateTime.Parse(dto.DateOfBirth.ToString()).ToShortDateString()}");
            }

            // If no changes, skip update
            if (!changes.Any()) {
                Console.WriteLine("No changes detected.");
                return ToDto(profile);
            }

            // Update profile details
            profile.UpdateDetails(new FirstName(dto.FirstName), new LastName(dto.LastName), new FullName(dto.FullName),
                dto.DateOfBirth, new Email(dto.Email), new ContactInformation(dto.ContactInformation));

            // Commit changes
            await _unitOfWork.CommitAsync();

            // Generate a changes summary
            string changesSummary = string.Join("<br/>", changes);

            // Send email notification with the changes
            await _emailService.SendProfileUpdateEmailAsync(profile.FirstName.Value, profile.LastName.Value, profile.Email.Value, changesSummary);

            // Return the updated profile as DTO
            return ToDto(profile);
        }

        
        // ---------------------------------------------------------------------------------------------------------------------
        //UPDATE METHOD - PATIENT
        
        public async Task<PatientProfileDTO> UpdateAsyncPatient(CreatingPatientProfileDto dto, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email must be provided.");
            }

            var profile = await _repo.FindByEmailAsync(email);
            if (profile == null)
            {
                return null;
            }

            // Check for unique email
            var existingProfileByEmail = await _repo.FindByEmailAndExcludedIdAsync(dto.Email, profile.Id);
            if (existingProfileByEmail != null)
            {
                throw new ArgumentException("A profile with the same email already exists.");
            }

            // Check for unique contact information
            var existingProfileByContact = await _repo.FindByPhoneNumberAsync(dto.ContactInformation);
            if (existingProfileByContact != null && existingProfileByContact.Id != profile.Id)
            {
                throw new ArgumentException("A profile with the same contact information already exists.");
            }

            profile.UpdateDetails(new FirstName(dto.FirstName), new LastName(dto.LastName), new FullName(dto.FullName),
                dto.DateOfBirth, new Email(dto.Email), new ContactInformation(dto.ContactInformation));
            await _unitOfWork.CommitAsync();

            await _updatedEventHandler.Handle(new PatientProfileUpdatedEvent(profile.Id.AsGuid(), profile.FullName,
                profile.Email));

            return ToDto(profile);
        }

        // ---------------------------------------------------------------------------------------------------------------------
        //DELETE METHOD

        public async Task<bool> DeleteAsync(string dateOfBirth = null, string medicalRecordNumber = null,
            string email = null, string fullName = null, string phoneNumber = null)
        {
            PatientProfile profile = null;

            if (string.IsNullOrWhiteSpace(medicalRecordNumber) && string.IsNullOrWhiteSpace(email) &&
                string.IsNullOrWhiteSpace(dateOfBirth) && string.IsNullOrWhiteSpace(fullName) &&
                string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException(
                    "At least one of the following parameters must be provided: medical record number, email, date of birth, phone number, or full name.");
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                profile = await _repo.FindByEmailAsync(email);
            }
            else if (!string.IsNullOrWhiteSpace(medicalRecordNumber))
            {
                profile = await _repo.FindByMedicalRecordNumberAsync(medicalRecordNumber);
            }
            else if (!string.IsNullOrWhiteSpace(dateOfBirth))
            {
                profile = await _repo.FindByDateOfBirthAsync(DateTime.Parse(dateOfBirth));
            }
            else if (!string.IsNullOrWhiteSpace(fullName))
            {
                profile = await _repo.FindByFullNameAsync(fullName);
            }
            else if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                profile = await _repo.FindByPhoneNumberAsync(long.Parse(phoneNumber));
            }

            if (profile == null)
            {
                return false;
            }

            // Mark the patient profile for deletion
            profile.MarkForDeletion();
            await _unitOfWork.CommitAsync();

            // Schedule the actual deletion using PatientDeletionService
            _queue.QueueAsyncTask(async () =>
            {
                await Task.Delay(TimeSpan.FromMinutes(2)); // Wait for 2 minutes
                await _deletionService.ConfirmAndDeleteAccountAsync(profile.Id, _repo, _unitOfWork, _logger);
            });

            return true;
        }

        // ---------------------------------------------------------------------------------------------------------------------
        //LIST  METHODS
        public async Task<List<PatientProfileDTO>> GetAllAsync(string email = null, string medicalRecordNumber = null,
            string fullName = null, string dateOfBirth = null, string phoneNumber = null) {
            List<PatientProfile> list;

            if (!string.IsNullOrWhiteSpace(email)) {
                list = new List<PatientProfile> { await _repo.FindByEmailAsync(email) };
            }else if (!string.IsNullOrWhiteSpace(medicalRecordNumber)) {
                list = new List<PatientProfile> { await _repo.FindByMedicalRecordNumberAsync(medicalRecordNumber) };
            }else if (!string.IsNullOrWhiteSpace(fullName)) {
                list = new List<PatientProfile> { await _repo.FindByFullNameAsync(fullName) };
            }else if (!string.IsNullOrWhiteSpace(dateOfBirth)) {
                list = new List<PatientProfile> { await _repo.FindByDateOfBirthAsync(DateTime.Parse(dateOfBirth)) };
            }else if (!string.IsNullOrWhiteSpace(phoneNumber)) {
                list = new List<PatientProfile> { await _repo.FindByPhoneNumberAsync(long.Parse(phoneNumber)) };
            }else {
                list = await _repo.GetAllAsync();
            }

            return list.ConvertAll(profile => new PatientProfileDTO {
                Id = profile.Id.AsGuid(),
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                FullName = profile.FullName,
                DateOfBirth = profile.DateOfBirth,
                Email = profile.Email,
                ContactInformation = profile.ContactInformation,
                Gender = profile.Gender,
                EmergencyContact = profile.EmergencyContact,
                MedicalRecordNumber = profile.MedicalRecordNumber,
                AllergiesMedicalCond = profile.AllergiesMedicalCond,
                AppointmentHistory = profile.AppointmentHistory
            });
        }

        public async Task<PatientProfileDTO> GetByIdAsync(Guid id)
        {
            var profile = await this._repo.GetByIdAsync(new PatientProfileId(id));

            if (profile == null)
                return null;

            return ToDto(profile);
        }


        private PatientProfileDTO ToDto(PatientProfile profile)
        {
            return new PatientProfileDTO
            {
                Id = profile.Id.AsGuid(),
                FirstName = profile.FirstName.Value,
                LastName = profile.LastName.Value,
                FullName = profile.FullName.Value,
                DateOfBirth = profile.DateOfBirth,
                Email = profile.Email.Value,
                ContactInformation = profile.ContactInformation.Value,
                MedicalRecordNumber = profile.MedicalRecordNumber
            };
        }
        
        // ---------------------------------------------------------------------------------------------------------------------
        //DELETE ACCOUNT METHODS BY PATIENT
        
        public async Task RequestAccountDeletionByEmailAsync(string email)
        {
            var patient = await _repo.FindByEmailAsync(email);
            if (patient == null)
            {
                throw new ArgumentException("Patient not found");
            }

            var token = Guid.NewGuid().ToString();
            _tokenManager.SetToken(token, email, TimeSpan.FromHours(1)); // Store token for 1 hour

            await _emailService.SendAccountDeletionConfirmationEmailAsync(email, token);
        }

        public async Task ConfirmAndDeleteAccountAsync(string token, string userEmail)
        {
            if (!_tokenManager.TryGetToken(token, out var email) || email != userEmail)
            {
                throw new ArgumentException("Invalid or expired token");
            }

            // Remove the token to ensure it can only be used once
            _tokenManager.RemoveToken(token);

            var patient = await _repo.FindByEmailAsync(email);
            if (patient == null)
            {
                throw new ArgumentException("Patient not found");
            }

            // Mark the patient profile for deletion
            patient.MarkForDeletion();
            await _unitOfWork.CommitAsync();

            // Schedule the actual deletion after 2 minutes using Coravel
            _queue.QueueAsyncTask(async () =>
            {
                await Task.Delay(TimeSpan.FromMinutes(2)); // Wait for 2 minutes
                await _deletionService.ConfirmAndDeleteAccountAsync(patient.Id, _repo, _unitOfWork, _logger);
            });

            // Optionally, send a notification email about the scheduled deletion
            await _emailService.SendAccountDeletionScheduledEmailAsync(patient.Email.Value, patient.DeletionScheduledDate.Value);
        }
    }
}