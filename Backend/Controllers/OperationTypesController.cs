using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DDDSample1.Domain.OperationTypes;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using DDDSample1.Domain.StaffProfile;
using Microsoft.Extensions.Logging;

namespace DDDNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Policy = "AdminsOrDoctors")]
    public class OperationTypesController : ControllerBase
    {
        private readonly OperationTypeService _service;
        private readonly ILogger<OperationTypesController> _logger;

        public OperationTypesController(OperationTypeService service, ILogger<OperationTypesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // POST: api/OperationTypes
        [HttpPost]
        public async Task<IActionResult> AddOperationType([FromBody] CreatingOperationTypeDto dto)
        {
            try
            {
                var result = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Failed to create operation type: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating operation type.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        
        // GET: api/OperationTypes
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<OperationTypeDTO>>> GetAll()
        {
            var operationTypes = await _service.GetAllAsync();
            return Ok(operationTypes);
        }


        // GET: api/OperationTypes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OperationTypeDTO>> GetById(Guid id)
        {
            var operationType = await _service.GetByIdAsync(id);
            
            if (operationType == null)
            {
                return NotFound();
            }

            return Ok(operationType);
        }

        // PUT: api/OperationTypes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOperationType(Guid id, [FromBody] CreatingOperationTypeDto operationTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _service.UpdateAsync(id, operationTypeDto);

            if (result == null)
            {
                return Conflict(new { Message = "Operation type with this name already exists or not found." });
            }

            return Ok(result);
        }

        // DELETE: api/OperationTypes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateOperationType(Guid id)
        {
            var isDeactivated = await _service.DeactivateAsync(id);
            
            if (!isDeactivated)
            {
                return NotFound(new { Message = "Operation type not found." });
            }

            return Ok(new { Message = "Operation type successfully deactivated." });
        }

        // GET: api/OperationTypes/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<OperationTypeDTO>>> Search(
            [FromQuery] Guid? id,
            [FromQuery] string name,
            [FromQuery] bool? isActive)
        {
            var operationTypes = await _service.SearchAsync(id, name, isActive);
            return Ok(operationTypes);
        }
    }
}