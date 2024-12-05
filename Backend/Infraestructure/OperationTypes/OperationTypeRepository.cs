using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Infrastructure.OperationTypes
{
    public class OperationTypeRepository : BaseRepository<OperationType, OperationTypeId>, IOperationTypeRepository
    {
        private readonly DDDSample1DbContext _context;

        public OperationTypeRepository(DDDSample1DbContext context) : base(context.OperationTypes)
        {
            _context = context; 
        }

        public async Task<OperationType> FindByNameAndExcludedIdAsync(Name name, OperationTypeId excludedId)
        {
            return await _context.OperationTypes
                .FirstOrDefaultAsync(op => op.Name == name && op.Id != excludedId);
        }

        // Not efficient because it loads all operation types from the database, 
        // but EF Like Function is not working for Name property 
        public async Task<List<OperationType>> SearchAsync(string name, string specialization, bool? isActive) 
        {
            var operationTypes = await _context.OperationTypes.ToListAsync();

            var filtered = operationTypes
                .Where(o => string.IsNullOrEmpty(name) || o.Name.Value.ToLower().Contains(name.ToLower()))
                .Where(op => !isActive.HasValue || op.isActive == isActive.Value)
                .ToList();

            return filtered;
        }

        public async Task<List<OperationType>> SearchAsync(OperationTypeId id, string name, bool? isActive)
        {
            var query = _context.OperationTypes.AsQueryable();

            if (id != null)
            {
                query = query.Where(op => op.Id == id);
            }

            if (isActive.HasValue)
            {
                query = query.Where(op => op.isActive == isActive.Value);
            }

            var list = await query.ToListAsync();

            if (!string.IsNullOrEmpty(name))
            {
                list = list.Where(op => op.Name.Value.Contains(name)).ToList();
            }

            return list;
        }

    }
}
