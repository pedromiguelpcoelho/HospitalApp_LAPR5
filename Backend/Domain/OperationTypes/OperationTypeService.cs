using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Events;
using DDDSample1.Domain.OperationTypes;
using System.Linq;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Events.Handlers;
using DDDSample1.Domain.StaffProfile;

namespace DDDSample1.Domain.OperationTypes
{
    public class OperationTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOperationTypeRepository _repo;
        private readonly OperationTypeCreatedEventHandler _eventHandler;

        public OperationTypeService(IUnitOfWork unitOfWork, IOperationTypeRepository repo, OperationTypeCreatedEventHandler eventHandler)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
            _eventHandler = eventHandler;
        }

        public async Task<List<OperationTypeDTO>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(ToDTO).ToList(); // Converte cada OperationType em DTO
        }

        public async Task<OperationTypeDTO> GetByIdAsync(Guid id)
        {
            var opType = await _repo.GetByIdAsync(new OperationTypeId(id));
            return opType == null ? null : ToDTO(opType);
        }

        public async Task<OperationTypeDTO> AddAsync(CreatingOperationTypeDto dto)
        {
            if (await CheckIfOperationTypeExists(dto.Name, Guid.Empty))
                throw new InvalidOperationException("Operation type with name " + dto.Name + " already exists.");

            var requiredStaffIds = dto.RequiredStaff.Select(staff => new StaffId(Guid.Parse(staff))).ToList();
            var operationType = new OperationType(new Name(dto.Name), requiredStaffIds, dto.EstimatedDuration);

            await _repo.AddAsync(operationType);
            await _unitOfWork.CommitAsync();

            var eventData = new OperationTypeCreatedEvent(operationType.Id.AsGuid(), operationType.Name.Value, operationType.RequiredStaff, operationType.EstimatedDuration);
            await _eventHandler.Handle(eventData);

            return ToDTO(operationType);
        }

        public async Task<bool> CheckIfOperationTypeExists(string name, Guid excludedId)
        {
            var existingOpType = await _repo.FindByNameAndExcludedIdAsync(new Name(name), new OperationTypeId(excludedId));
            return existingOpType != null;
        }

        public async Task<OperationTypeDTO> UpdateAsync(Guid id, CreatingOperationTypeDto dto)
        {
            var opType = await _repo.GetByIdAsync(new OperationTypeId(id));

            if (opType == null || await CheckIfOperationTypeExists(dto.Name, id))
                return null;  // Retorna null se não encontrado ou se o nome já existe em outro tipo de operação.

            var requiredStaffIds = dto.RequiredStaff.Select(staff => new StaffId(Guid.Parse(staff))).ToList();
            opType.UpdateDetails(new Name(dto.Name), requiredStaffIds, dto.EstimatedDuration);

            await _unitOfWork.CommitAsync();

            return ToDTO(opType);
        }

        public async Task<OperationTypeDTO> DeleteAsync(OperationTypeId id)
        {
            var opType = await _repo.GetByIdAsync(id);
            if (opType == null)
                return null;

            _repo.Remove(opType);
            await _unitOfWork.CommitAsync();

            return ToDTO(opType);
        }

        public async Task<bool> DeactivateAsync(Guid id)
        {
            var operationType = await _repo.GetByIdAsync(new OperationTypeId(id));
            if (operationType == null)
                return false;

            operationType.Deactivate();
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<List<OperationTypeDTO>> SearchAsync(Guid? id, string name, bool? isActive)
    {
        var operationTypeId = id.HasValue ? new OperationTypeId(id.Value) : null;
        var list = await _repo.SearchAsync(operationTypeId, name, isActive);
        return list.Select(ToDTO).ToList();
    }

        // Método privado para converter OperationType em OperationTypeDTO
        private OperationTypeDTO ToDTO(OperationType operationType)
        {
            return new OperationTypeDTO
            {
                Id = operationType.Id.AsGuid(),
                Name = operationType.Name.Value,
                RequiredStaff = operationType.RequiredStaff.Select(rs => rs.AsString()).ToList(),  // Converter StaffId para string
                EstimatedDuration = operationType.EstimatedDuration,
                isActive = operationType.isActive
            };
        }
    }
}