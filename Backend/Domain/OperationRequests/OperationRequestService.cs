using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Events;
using DDDSample1.Domain.Events.Handlers;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.StaffProfile;

namespace DDDSample1.Domain.OperationRequests
{
    public class OperationRequestService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IOperationRequestRepository repoOr;
        private readonly IOperationTypeRepository repoOt;
        private readonly IPatientProfileRepository repoP;
        private readonly IStaffRepository repoS;
        private readonly OperationRequestCreatedEventHandler createdEventHandler;
        private readonly OperationRequestDeletedEventHandler deletedEventHandler;
        private readonly OperationRequestUpdatedEventHandler updatedEventHandler;

        public OperationRequestService(
            IUnitOfWork unitOfWork,
            IOperationRequestRepository repoOr,
            IOperationTypeRepository repoOt,
            IPatientProfileRepository repoP,
            IStaffRepository repoS,
            OperationRequestCreatedEventHandler createdEventHandler,
            OperationRequestDeletedEventHandler deletedEventHandler,
            OperationRequestUpdatedEventHandler updatedEventHandler)
        {
            this.unitOfWork = unitOfWork;
            this.repoOr = repoOr;
            this.repoOt = repoOt;
            this.repoP = repoP;
            this.repoS = repoS;
            this.createdEventHandler = createdEventHandler;
            this.deletedEventHandler = deletedEventHandler;
            this.updatedEventHandler = updatedEventHandler;
        }

        public async Task<List<OperationRequestDto>> GetAllAsync()
        {
            var list = await repoOr.GetAllAsync();
            return await MapToDtoList(list);
        }

        public async Task<OperationRequestDto> GetByIdAsync(Guid id)
        {
            var operationRequest = await repoOr.GetByIdAsync(new OperationRequestId(id));
            return operationRequest == null ? null : await MapToDto(operationRequest);
        }

        public async Task<OperationRequestDto> AddAsync(CreatingOperationRequestDto dto)
        {
            if (dto.SuggestedDeadline <= DateTime.UtcNow)
            {
                throw new OperationRequestExceptions.InvalidSuggestedDeadlineException();
            }

            await ValidateNoOpenRequestAsync(dto.PatientId, dto.OperationTypeId);

            var operationRequest = new OperationRequest(
                new PatientProfileId(dto.PatientId), 
                new StaffId(dto.DoctorId), 
                new OperationTypeId(dto.OperationTypeId), 
                dto.Priority, 
                dto.SuggestedDeadline);

            await ValidateOperationTypeIdAsync(dto.OperationTypeId);
            await ValidatePatientIdAsync(dto.PatientId);
            await ValidateStaffIdAsync(dto.DoctorId);

            await repoOr.AddAsync(operationRequest);
            await unitOfWork.CommitAsync();

            await createdEventHandler.Handle(new OperationRequestCreatedEvent(
                operationRequest.Id.AsGuid(), 
                operationRequest.PatientId.AsGuid(), 
                operationRequest.DoctorId.AsGuid(), 
                operationRequest.OperationTypeId.AsGuid(), 
                operationRequest.Priority, 
                operationRequest.SuggestedDeadline));
            
            // Log the operation request in the patient's medical history
            var patient = await repoP.GetByIdAsync(new PatientProfileId(dto.PatientId));
            var operationType = await repoOt.GetByIdAsync(new OperationTypeId(dto.OperationTypeId));

            patient.AppendToAppointmentHistory(operationType.Name, dto.SuggestedDeadline);
            await unitOfWork.CommitAsync();

            return await MapToDto(operationRequest);
        }

        public async Task<OperationRequestDto> UpdateAsync(UpdatingOperationRequestDto dto)
        {
            var operationRequest = await repoOr.GetByIdAsync(new OperationRequestId(dto.Id));
            if (operationRequest == null)
            {
                return null; // Return null if the operation request is not found or the doctor is not authorized
            }

            if (dto.SuggestedDeadline <= DateTime.UtcNow)
            {
                throw new OperationRequestExceptions.InvalidSuggestedDeadlineException();
            }

            var originalPriority = operationRequest.Priority;
            var originalDeadline = operationRequest.SuggestedDeadline;

            // Retrieve the patient profile and operation type
            var patient = await repoP.GetByIdAsync(operationRequest.PatientId);
            var operationType = await repoOt.GetByIdAsync(operationRequest.OperationTypeId);

            // Remove the old appointment entry
            patient.RemoveFromAppointmentHistory(operationType.Name, originalDeadline);

            // Update the operation request
            operationRequest.UpdatePriority(dto.Priority);
            operationRequest.UpdateSuggestedDeadline(dto.SuggestedDeadline);

            // Add the new appointment entry
            patient.AppendToAppointmentHistory(operationType.Name, dto.SuggestedDeadline);

            await unitOfWork.CommitAsync();

            // Handle the update event
            await updatedEventHandler.Handle(new OperationRequestUpdatedEvent(
                operationRequest.Id.AsGuid(),
                originalPriority,
                dto.Priority,
                originalDeadline,
                dto.SuggestedDeadline
            ));

            return await MapToDto(operationRequest);
        }

        public async Task<string> UpdateOperationRequesitionAsync(string id, string userEmail, UpdatingOperationRequestDto dto)
        {
            var requisition = await repoOr.GetByIdAsync(new OperationRequestId(Guid.Parse(id)));

            if (requisition == null)
            {
                throw new Exception("Operation request not found.");
            }

            var user = await repoS.GetByEmailAsync(userEmail);

            if (user == null)
            {
                throw new Exception("Invalid user email.");
            }

            var doctorProfile = await repoS.GetByIdAsync(user.Id);

            if (requisition.DoctorId != doctorProfile.Id)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this operation request.");
            }

            // Retrieve the patient profile and operation type
            var patient = await repoP.GetByIdAsync(requisition.PatientId);
            var operationType = await repoOt.GetByIdAsync(requisition.OperationTypeId);

            // Remove the old appointment entry
            patient.RemoveFromAppointmentHistory(operationType.Name, requisition.SuggestedDeadline);

            // Update the operation request
            requisition.UpdateSuggestedDeadline(dto.SuggestedDeadline);
            requisition.UpdatePriority(dto.Priority);

            // Add the new appointment entry
            patient.AppendToAppointmentHistory(operationType.Name, dto.SuggestedDeadline);

            await unitOfWork.CommitAsync();

            await updatedEventHandler.Handle(new OperationRequestUpdatedEvent(
                requisition.Id.AsGuid(),
                requisition.Priority,
                dto.Priority,
                requisition.SuggestedDeadline,
                dto.SuggestedDeadline
            ));

            return "Operation request successfully updated.";
        }

        public async Task<bool> DeleteAsync(Guid operationRequestId)
        {
            var operationRequest = await repoOr.GetByIdAsync(new OperationRequestId(operationRequestId));

            if (operationRequest == null)
            {
                return false; // Return false if the operation request is not found or the doctor is not authorized
            }

            // Retrieve the patient profile and operation type
            var patient = await repoP.GetByIdAsync(operationRequest.PatientId);
            var operationType = await repoOt.GetByIdAsync(operationRequest.OperationTypeId);

            // Remove the appointment entry if the patient exists
            if (patient != null && operationType != null)
            {
                patient.RemoveFromAppointmentHistory(operationType.Name, operationRequest.SuggestedDeadline);
            }

            repoOr.Remove(operationRequest);
            await unitOfWork.CommitAsync();

            await deletedEventHandler.Handle(new OperationRequestDeletedEvent(operationRequest.Id.AsGuid()));

            return true; // Return true if the operation request was successfully deleted
        }

        public async Task<string> DeleteOperationRequesitionAsync(string id, string userEmail)
        {
            var operationRequest = await repoOr.GetByIdAsync(new OperationRequestId(Guid.Parse(id)));

            if (operationRequest == null)
            {
                throw new Exception("Operation request not found.");
            }

            var user = await repoS.GetByEmailAsync(userEmail);

            if (user == null)
            {
                throw new Exception("Invalid user email.");
            }

            var doctorProfile = await repoS.GetByIdAsync(user.Id);

            if (operationRequest.DoctorId != doctorProfile.Id)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this operation request.");
            }

            // Retrieve the patient profile and operation type
            var patient = await repoP.GetByIdAsync(operationRequest.PatientId);
            var operationType = await repoOt.GetByIdAsync(operationRequest.OperationTypeId);

            // Remove the appointment entry if the patient exists
            if (patient != null && operationType != null)
            {
                patient.RemoveFromAppointmentHistory(operationType.Name, operationRequest.SuggestedDeadline);
            }

            repoOr.Remove(operationRequest);
            await unitOfWork.CommitAsync();

            return "Operation request successfully deleted.";
        }

        public async Task<List<OperationRequestDto>> SearchAsync(
            string doctorName, 
            Priority? priority, 
            string operationTypeName, 
            string patientName, 
            DateTime? expectedDueDate,
            DateTime? requestDate)
        {
            var operationRequests = await repoOr.SearchAsync(doctorName, priority, operationTypeName, patientName, expectedDueDate, requestDate);
            return await MapToDtoList(operationRequests);
        }

        public async Task<List<OperationRequestDto>> SearchByDoctorAsync(
            string userEmail,
            string doctorName, 
            Priority? priority, 
            string operationTypeName, 
            string patientName, 
            DateTime? expectedDueDate,
            DateTime? requestDate)
        {
            var user = await repoS.GetByEmailAsync(userEmail);

            if (user == null)
            {
                throw new Exception("Invalid user email.");
            }

            var doctorProfile = await repoS.GetByIdAsync(user.Id);

            // Extract the Guid value from StaffId
            var doctorId = doctorProfile.Id.AsGuid();

            var operationRequests = await repoOr.SearchORAsync(
                doctorId,
                doctorName, 
                priority, 
                operationTypeName, 
                patientName, 
                expectedDueDate, 
                requestDate);

            return await MapToDtoList(operationRequests);
        }

        private async Task ValidateOperationTypeIdAsync(Guid operationTypeId)
        {
            if (await repoOt.GetByIdAsync(new OperationTypeId(operationTypeId)) == null)
            {
                throw new OperationRequestExceptions.InvalidOperationTypeIdException();
            }
        }

        private async Task ValidatePatientIdAsync(Guid patientId)
        {
            if (await repoP.GetByIdAsync(new PatientProfileId(patientId)) == null)
            {
                throw new OperationRequestExceptions.InvalidPatientIdException();
            }
        }
        
        private async Task ValidateStaffIdAsync(Guid staffId)
        {
            if (await repoS.GetByIdAsync(new StaffId(staffId)) == null)
            {
                throw new OperationRequestExceptions.InvalidDoctorIdException();
            }
        }
        
        private async Task ValidateNoOpenRequestAsync(Guid patientId, Guid operationTypeId)
        {
            var existingRequests = await repoOr.SearchAsync(patientId, operationTypeId);
            if (existingRequests != null && existingRequests.Any())
            {
                throw new OperationRequestExceptions.DuplicateOpenRequestException();
            }
        }

        private async Task<OperationRequestDto> MapToDto(OperationRequest operationRequest)
        {
            var patient = await repoP.GetByIdAsync(operationRequest.PatientId);
            var doctor = await repoS.GetByIdAsync(operationRequest.DoctorId);
            var operationType = await repoOt.GetByIdAsync(operationRequest.OperationTypeId);

            return new OperationRequestDto
            {
                Id = operationRequest.Id.AsGuid(),
                PatientId = operationRequest.PatientId.AsGuid(),
                PatientName = patient?.FullName.Value ?? "Unknown",
                DoctorId = operationRequest.DoctorId.AsGuid(),
                DoctorName = doctor?.Name.Value ?? "Unknown",
                OperationTypeId = operationRequest.OperationTypeId.AsGuid(),
                OperationTypeName = operationType?.Name.Value ?? "Unknown",
                Priority = operationRequest.Priority,
                SuggestedDeadline = operationRequest.SuggestedDeadline,
                RequestDate = operationRequest.RequestDate
            };
        }

        private async Task<List<OperationRequestDto>> MapToDtoList(List<OperationRequest> operationRequests)
        {
            var dtoList = new List<OperationRequestDto>();
            foreach (var request in operationRequests)
            {
                dtoList.Add(await MapToDto(request));
            }
            return dtoList;
        }
    }
}