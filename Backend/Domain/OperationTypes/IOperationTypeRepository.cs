using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.OperationTypes
{
    public interface IOperationTypeRepository: IRepository<OperationType, OperationTypeId>
    {
        Task<OperationType> FindByNameAndExcludedIdAsync(Name name, OperationTypeId excludedId);

        public Task<List<OperationType>> SearchAsync(OperationTypeId id, string name, bool? isActive); 
    }
    
}   