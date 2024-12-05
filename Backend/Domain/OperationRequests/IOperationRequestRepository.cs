using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.OperationRequests
{
    public interface IOperationRequestRepository : IRepository<OperationRequest, OperationRequestId>
    {
        Task<List<OperationRequest>> SearchAsync(
            string doctorName, 
            Priority? priority, 
            string operationTypeName, 
            string patientName, 
            DateTime? expectedDueDate,
            DateTime? requestDate);

        Task<List<OperationRequest>> SearchORAsync(
            Guid doctorId, // Use Guid instead of StaffId
            string doctorName, 
            Priority? priority, 
            string operationTypeName, 
            string patientName, 
            DateTime? expectedDueDate,
            DateTime? requestDate);
        
        Task<List<OperationRequest>> SearchAsync(Guid patientId, Guid operationTypeId);
    }
}