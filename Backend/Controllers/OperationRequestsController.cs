using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DDDSample1.Domain.OperationRequests;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;

namespace DDDNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OperationRequestsController : ControllerBase
    {
        private readonly OperationRequestService _service;

        public OperationRequestsController(OperationRequestService service)
        {
            _service = service;
        }

        [Authorize(Policy = "DoctorsOnly")]
        [HttpPost("add")]
        public async Task<IActionResult> AddOperationRequest(CreatingOperationRequestDto dto)
        {
            try
            {
                var result = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, new { message = "Operation request successfully added.", result });
            }
            catch (OperationRequestExceptions.DuplicateOpenRequestException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OperationRequestDto>> GetById(Guid id)
        {
            var operationRequest = await _service.GetByIdAsync(id);
            if (operationRequest == null)
            {
                return NotFound(new { message = "Operation request not found." });
            }
            return Ok(new { message = "Operation request successfully retrieved.", operationRequest });
        }

        [Authorize(Policy = "AdminsOnly")]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateOperationRequest(UpdatingOperationRequestDto dto)
        {
            var result = await _service.UpdateAsync(dto);
            if (result == null)
            {
                return BadRequest(new { message = "Failed to update operation request." });
            }
            return Ok(new { message = "Operation request successfully updated.", result });
        }

        [Authorize(Policy = "DoctorsOnly")]
        [HttpPatch("update/{id}")]
        public async Task<ActionResult> UpdateOperationRequesition(string id, [FromBody] UpdatingOperationRequestDto dto) 
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(new { message = "User email not found." });
            }

            try {
                var resultMessage = await _service.UpdateOperationRequesitionAsync(id, userEmail, dto);
                return Ok(resultMessage);
            }
            catch (UnauthorizedAccessException e) {
                return Unauthorized(e.Message);
            }
        }

        [Authorize(Policy = "AdminsOnly")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteOperationRequest(Guid operationRequestId)
        {
            var isDeleted = await _service.DeleteAsync(operationRequestId);
            if (!isDeleted)
            {
                return BadRequest(new { message = "Failed to delete operation request." });
            }
            return Ok(new { message = "Operation request successfully deleted." });
        }

        [Authorize(Policy = "DoctorsOnly")]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteOperationRequesition(string id)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(new { message = "User email not found." });
            }

            try
            {
                var resultMessage = await _service.DeleteOperationRequesitionAsync(id, userEmail);
                return Ok(resultMessage);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
        }

        [Authorize(Policy = "AdminsOnly")]
        [HttpGet("search")]
        public async Task<ActionResult<List<OperationRequestDto>>> Search(
            [FromQuery] string doctorName,
            [FromQuery] Priority? priority,
            [FromQuery] string operationTypeName,
            [FromQuery] string patientName,
            [FromQuery] DateTime? expectedDueDate,
            [FromQuery] DateTime? requestDate)
        {
            var operationRequests = await _service.SearchAsync(doctorName, priority, operationTypeName, patientName, expectedDueDate, requestDate);
            if (operationRequests == null || operationRequests.Count == 0)
            {
                return NotFound(new { message = "No operation requests found matching the search criteria." });
            }
            return Ok(new { message = "Operation requests successfully retrieved.", operationRequests });
        }

        [Authorize(Policy = "DoctorsOnly")]
        [HttpGet("searchByDoctor")]
        public async Task<ActionResult<List<OperationRequestDto>>> SearchByDoctor(
            [FromQuery] string doctorName,
            [FromQuery] Priority? priority,
            [FromQuery] string operationTypeName,
            [FromQuery] string patientName,
            [FromQuery] DateTime? expectedDueDate,
            [FromQuery] DateTime? requestDate)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(new { message = "User email not found." });
            }

            try
            {
                var operationRequests = await _service.SearchByDoctorAsync(userEmail, doctorName, priority, operationTypeName, patientName, expectedDueDate, requestDate);
                if (operationRequests == null || operationRequests.Count == 0)
                {
                    return NotFound(new { message = "You have no Operation Requests at this time!" });
                }
                return Ok(new { message = "Operation requests successfully retrieved.", operationRequests });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        
        [Authorize (Policy = "AdminsOrDoctors")]   
        [HttpGet("all")]
        public async Task<ActionResult<List<OperationRequestDto>>> GetAll()
        {
            var operationRequests = await _service.GetAllAsync();
            if (operationRequests == null || operationRequests.Count == 0)
            {
                return NotFound(new { message = "No operation requests found." });
            }
            return Ok(new { message = "All operation requests successfully retrieved.", operationRequests });
        }
    }
}