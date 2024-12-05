using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.StaffProfile;
using DDDSample1.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Infrastructure.OperationRequests
{
    public class OperationRequestRepository : BaseRepository<OperationRequest, OperationRequestId>, IOperationRequestRepository
    {
        private readonly DDDSample1DbContext _context;

        public OperationRequestRepository(DDDSample1DbContext context) : base(context.OperationRequests)
        {
            _context = context;
        }

        public async Task<List<OperationRequest>> SearchAsync(
            string doctorName, 
            Priority? priority, 
            string operationTypeName, 
            string patientName, 
            DateTime? expectedDueDate,
            DateTime? requestDate)
        {
            var query = _context.OperationRequests.AsQueryable();

            if (!string.IsNullOrEmpty(doctorName))
            {
                var doctorIds = _context.Staffs
                    .AsEnumerable()
                    .Where(s => s.Name.Value.Contains(doctorName))
                    .Select(s => s.Id)
                    .ToList();

                query = query.Where(or => doctorIds.Contains(or.DoctorId));
            }

            if (priority.HasValue)
            {
                query = query.Where(or => or.Priority == priority.Value);
            }

            if (!string.IsNullOrEmpty(operationTypeName))
            {
                var operationTypeIds = _context.OperationTypes
                    .AsEnumerable()
                    .Where(ot => ot.Name.Value.Contains(operationTypeName))
                    .Select(ot => ot.Id)
                    .ToList();

                query = query.Where(or => operationTypeIds.Contains(or.OperationTypeId));
            }

            if (!string.IsNullOrEmpty(patientName))
            {
                var patientIds = _context.PatientProfiles
                    .AsEnumerable()
                    .Where(p => p.FirstName.Value.Contains(patientName) || p.LastName.Value.Contains(patientName) || p.FullName.Value.Contains(patientName))
                    .Select(p => p.Id)
                    .ToList();

                query = query.Where(or => patientIds.Contains(or.PatientId));
            }

            if (expectedDueDate.HasValue)
            {
                query = query.Where(or => or.SuggestedDeadline <= expectedDueDate.Value);
            }

            if (requestDate.HasValue)
            {
                query = query.Where(or => or.RequestDate <= requestDate.Value);
            }

            return await query.ToListAsync();
        }
        
        public async Task<List<OperationRequest>> SearchAsync(Guid patientId, Guid operationTypeId)
        {
            return await _context.OperationRequests
                .Where(or => or.PatientId == new PatientProfileId(patientId) && or.OperationTypeId == new OperationTypeId(operationTypeId))
                .ToListAsync();
        }

        public async Task<List<OperationRequest>> SearchORAsync(
            Guid doctorId, // Use Guid instead of StaffId
            string doctorName, 
            Priority? priority, 
            string operationTypeName, 
            string patientName, 
            DateTime? expectedDueDate,
            DateTime? requestDate)
        {
            var query = _context.OperationRequests
                .Where(or => or.DoctorId == new StaffId(doctorId));

            if (!string.IsNullOrEmpty(doctorName))
            {
                var doctorIds = _context.Staffs
                    .AsEnumerable()
                    .Where(s => s.Name.Value.Contains(doctorName))
                    .Select(s => s.Id)
                    .ToList();

                query = query.Where(or => doctorIds.Contains(or.DoctorId));
            }

            if (priority.HasValue)
            {
                query = query.Where(or => or.Priority == priority.Value);
            }

            if (!string.IsNullOrEmpty(operationTypeName))
            {
                var operationTypeIds = _context.OperationTypes
                    .AsEnumerable()
                    .Where(ot => ot.Name.Value.Contains(operationTypeName))
                    .Select(ot => ot.Id)
                    .ToList();

                query = query.Where(or => operationTypeIds.Contains(or.OperationTypeId));
            }

            if (!string.IsNullOrEmpty(patientName))
            {
                var patientIds = _context.PatientProfiles
                    .AsEnumerable()
                    .Where(p => p.FirstName.Value.Contains(patientName) || p.LastName.Value.Contains(patientName) || p.FullName.Value.Contains(patientName))
                    .Select(p => p.Id)
                    .ToList();

                query = query.Where(or => patientIds.Contains(or.PatientId));
            }

            if (expectedDueDate.HasValue)
            {
                query = query.Where(or => or.SuggestedDeadline <= expectedDueDate.Value);
            }

            if (requestDate.HasValue)
            {
                query = query.Where(or => or.RequestDate <= requestDate.Value);
            }

            return await query.ToListAsync();
        }
    }
}