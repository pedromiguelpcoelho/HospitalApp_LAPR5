using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Events;
using DDDSample1.Domain.Events.Handlers;
using DDDSample1.Domain.Shared;
using DDDNetCore.Domain.Email;

namespace DDDSample1.Domain.StaffProfile
{
    public class StaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStaffRepository _repo;

        private readonly IEmailService _emailService;
        private readonly StaffCreatedEventHandler _createdEventHandler;
        private readonly StaffDeletedEventHandler _deletedEventHandler;

        private readonly StaffUpdatedEventHandler _updatedEventHandler;


        public StaffService(IUnitOfWork unitOfWork, IStaffRepository repo,IEmailService emailService, StaffCreatedEventHandler createdEventHandler, StaffDeletedEventHandler deletedEventHandler,StaffUpdatedEventHandler updatedEventHandler)
        {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._emailService = emailService;
            this._createdEventHandler = createdEventHandler;
            this._deletedEventHandler = deletedEventHandler;
            this._updatedEventHandler = updatedEventHandler;
        }


        // Method to retrieve all staff profiles (with optional filters)
        public async Task<List<StaffDTO>> SearchStaffProfilesAsync(string name ,string email,string phoneNumber, string specialization , string licenseNumber ,  bool? isActive = null )
        {
            var staffList = await this._repo.SearchStaffProfilesAsync(name, email,phoneNumber,specialization, licenseNumber, isActive);

            List<StaffDTO> listDto = staffList.ConvertAll(staff => new StaffDTO
            {
                Id = staff.Id.AsGuid(),                           
                FirstName = staff.FirstName.Value,                
                LastName = staff.LastName.Value,                  
                Name = staff.Name.Value,                           
                Role = staff.Role.Value,                           
                LicenseNumber = staff.LicenseNumber.Value,        
                Specialization = staff.Specialization,             
                Email = staff.Email.Value,                         
                PhoneNumber = staff.PhoneNumber.Value,             
                IsActive = staff.IsActive   
            });

            return listDto;
        }

        // Method to retrieve all staff profiles
        public async Task<List<StaffDTO>> GetAllAsync()
        {            var list = await this._repo.GetAllAsync();

            List<StaffDTO> listDto = list.ConvertAll<StaffDTO>(staff => new StaffDTO
            {
                Id = staff.Id.AsGuid(),                           
                FirstName = staff.FirstName.Value,                
                LastName = staff.LastName.Value,                  
                Name = staff.Name.Value,                           
                Role = staff.Role.Value,                           
                LicenseNumber = staff.LicenseNumber.Value,        
                Specialization = staff.Specialization,             
                Email = staff.Email.Value,                         
                PhoneNumber = staff.PhoneNumber.Value,             
                IsActive = staff.IsActive  
            });

            return listDto;
        }

        // Method to retrieve a staff profile by ID
        public async Task<StaffDTO> GetByIdAsync(Guid id)
        {
            var staff = await this._repo.GetByIdAsync(new StaffId(id));

            if (staff == null)
                return null;

            return new StaffDTO
            {
                Id = staff.Id.AsGuid(),                           
                FirstName = staff.FirstName.Value,                
                LastName = staff.LastName.Value,                  
                Name = staff.Name.Value,                           
                Role = staff.Role.Value,                           
                LicenseNumber = staff.LicenseNumber.Value,        
                Specialization = staff.Specialization,             
                Email = staff.Email.Value,                         
                PhoneNumber = staff.PhoneNumber.Value,             
                IsActive = staff.IsActive  
            };
        }

        // Method to retrieve a staff profile by Email
        public async Task<StaffDTO> GetByEmailAsync(string email)
        {
            var staff = await this._repo.GetByEmailAsync(email); 

            if (staff == null)
                return null;

            return new StaffDTO
            {
                 Id = staff.Id.AsGuid(),                           
                FirstName = staff.FirstName.Value,                
                LastName = staff.LastName.Value,                  
                Name = staff.Name.Value,                           
                Role = staff.Role.Value,                           
                LicenseNumber = staff.LicenseNumber.Value,        
                Specialization = staff.Specialization,             
                Email = staff.Email.Value,                         
                PhoneNumber = staff.PhoneNumber.Value,             
                IsActive = staff.IsActive   
            };
        }



        // Method to retrieve a staff profile by Phone Number
        public async Task<StaffDTO> GetByPhoneNumberAsync(string phoneNumber)
        {
            var staff = await this._repo.GetByPhoneNumberAsync(phoneNumber); 

            if (staff == null)
            return null;
            
            return new StaffDTO
            {
                Id = staff.Id.AsGuid(),                           
                FirstName = staff.FirstName.Value,                
                LastName = staff.LastName.Value,                  
                Name = staff.Name.Value,                           
                Role = staff.Role.Value,                           
                LicenseNumber = staff.LicenseNumber.Value,        
                Specialization = staff.Specialization,             
                Email = staff.Email.Value,                         
                PhoneNumber = staff.PhoneNumber.Value,             
                IsActive = staff.IsActive                        
            };
        }



        // Method to add a new staff profile
        public async Task<StaffDTO> AddAsync(CreatingStaffDto dto)
        {
            var staff = new Staff(dto.FirstName,dto.LastName, dto.Role , dto.Specialization, dto.Email, dto.PhoneNumber);

            await this._repo.AddAsync(staff);
            await this._unitOfWork.CommitAsync();

            return new StaffDTO
            {
                Id = staff.Id.AsGuid(),                           
                FirstName = staff.FirstName.Value,                
                LastName = staff.LastName.Value,                  
                Name = staff.Name.Value,                           
                Role = staff.Role.Value,                           
                LicenseNumber = staff.LicenseNumber.Value,        
                Specialization = staff.Specialization,             
                Email = staff.Email.Value,                         
                PhoneNumber = staff.PhoneNumber.Value,             
                IsActive = staff.IsActive  
            };
        }

        // Method to update an existing staff profile
        public async Task<StaffDTO> UpdateAsync(Guid id, CreatingStaffDto dto)
        {
            var staff = await this._repo.GetByIdAsync(new StaffId(id));

            if (staff == null)
                return null;

            
            bool hasChanges = false;
            var changes = new List<string>(); 

                if (!string.Equals(staff.FirstName.Value, dto.FirstName, StringComparison.Ordinal))
                {
            
                changes.Add($"First Name\nOld: {staff.FirstName.Value}\nNew: {dto.FirstName}\n\n");
                staff.UpdateFirstName(new FirstName(dto.FirstName));
                hasChanges = true;
                }

               
                if (!string.Equals(staff.LastName.Value, dto.LastName, StringComparison.Ordinal))
                {
                     changes.Add($"Last Name\n\n\nOld: {staff.LastName.Value}\n\n\nNew: {dto.LastName}\n\n");
                    staff.UpdateLastName(new LastName(dto.LastName));
                    hasChanges = true;
                }

               
                if (!string.Equals(staff.Email.Value, dto.Email, StringComparison.Ordinal))
                {
                    var existingStaffWithEmail = await _repo.GetByEmailAsync(dto.Email);
                    if (existingStaffWithEmail != null && existingStaffWithEmail.Id != staff.Id)
                    {
                        throw new InvalidOperationException("An employee with this email already exists.");
                    }
                  changes.Add($"Email Address\n\n\nOld: {staff.Email.Value}\n\n\nNew: {dto.Email}\n\n");
                     staff.UpdateEmail(new Email(dto.Email));
                    hasChanges = true;
                }

               
                if (!string.Equals(staff.PhoneNumber.Value, dto.PhoneNumber, StringComparison.Ordinal))
                {
                    var existingStaffWithPhoneNumber = await _repo.GetByPhoneNumberAsync(dto.PhoneNumber);
                    if (existingStaffWithPhoneNumber != null && existingStaffWithPhoneNumber.Id != staff.Id)
                    {
                        throw new InvalidOperationException("An employee with this phone number already exists.");
                    }
                    changes.Add($"Phone Number\n\n\nOld: {staff.PhoneNumber.Value}\n\n\nNew: {dto.PhoneNumber}\n\n");
                   staff.UpdatePhoneNumber(new PhoneNumber(dto.PhoneNumber));
                    hasChanges = true;
                }

                
                if (!string.Equals(staff.Role.Value, dto.Role, StringComparison.Ordinal))
                {
                    changes.Add($"Role\n\n\nOld: {staff.Role.Value}\n\n\nNew: {dto.Role}\n\n");
                   staff.UpdateRole(new Role(dto.Role));
                    hasChanges = true;
                }

                if (!string.Equals(staff.Specialization, dto.Specialization, StringComparison.Ordinal))
                {
                      changes.Add($"Specialization\n\n\nOld: {staff.Specialization}\n\n\nNew: {dto.Specialization}\n\n");
                    staff.UpdateSpecialization(new Specialization(dto.Specialization,dto.Role));
                    hasChanges = true;
                }


        
            if (hasChanges)
        {
            await this._unitOfWork.CommitAsync();
             string changesSummary = string.Join("<br/>", changes);
            await _emailService.SendStaffUpdateEmailAsync(staff.FirstName.Value, staff.LastName.Value, staff.Email.Value,changesSummary);
            await _updatedEventHandler.Handle(new StaffUpdatedEvent(
                staff.Id.AsGuid(),
                staff.Name,
                staff.Role,
                staff.Specialization,
                staff.Email,
                staff.PhoneNumber
            ));
        }

            return new StaffDTO
            {
                Id = staff.Id.AsGuid(),                           
                FirstName = staff.FirstName.Value,                
                LastName = staff.LastName.Value,                  
                Name = staff.Name.Value,                           
                Role = staff.Role.Value,                           
                LicenseNumber = staff.LicenseNumber.Value,        
                Specialization = staff.Specialization,             
                Email = staff.Email.Value,                         
                PhoneNumber = staff.PhoneNumber.Value,             
                IsActive = staff.IsActive   
            };

        

        }

        // Method to check if a staff member with a specific license number already exists (except for a given staff id)
        public async Task<bool> CheckIfStaffExists(string email, Guid excludedId)
        {
            var existingStaff = await this._repo.FindByEmailAndExcludedIdAsync(email, new StaffId(excludedId));
            return existingStaff != null;
        }

        // Method to deactivate a staff profile
        public async Task<bool> DeactivateAsync(Guid id)
        {
            var staff = await this._repo.GetByIdAsync(new StaffId(id));
            if (staff == null)
            {
                return false;
            }

            staff.Deactivate();
            await this._unitOfWork.CommitAsync();

            return true;
        }

        // Method to delete a staff profile
        public async Task<bool> DeleteAsync(Guid id)
        {
            var staff = await this._repo.GetByIdAsync(new StaffId(id));

            if (staff == null)
                return false;

            this._repo.Remove(staff);
            await this._unitOfWork.CommitAsync();

            return true;
        }
    }
}
